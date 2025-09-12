#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

// All-in-one toolbox that auto-hooks to existing user scripts: EffectUIList, EditorCircleView, PlayTest, CustomEditorSystem.
public class LopToolboxWindow : EditorWindow
{
    private enum Tab { Effects, CirclePaint, PlayTest }
    private Tab _tab = Tab.Effects;

    // ---------- Shared ----------
    private SerializedObject _so;
    private Vector2 _scroll;

    // ---------- Effects ----------
    private UnityEngine.Object _effectTarget;   // e.g., a GameObject with EffectUIList or ScriptableObject
    private string[] _effectMethodNames = Array.Empty<string>();
    private MethodInfo[] _effectMethods = Array.Empty<MethodInfo>();
    private string _effectFilter = "";
    private Transform _effectSpawnParent;
    private GameObject _fallbackEffectPrefab;
    private float _effectPreviewDuration = 1.5f;

    // ---------- Circle Paint ----------
    private GameObject _paintPrefab;
    private Transform _paintCenter;
    private float _radius = 3f;
    private int _countOnCircle = 12;
    private bool _snapToGround = true;
    private LayerMask _groundMask = ~0;
    private int _seed = 1234;

    // ---------- PlayTest ----------
    private UnityEngine.Object _playTestTarget;   // e.g., a GameObject with PlayTest
    private string[] _playTestCallableNames = Array.Empty<string>();
    private MethodInfo[] _playTestCallables = Array.Empty<MethodInfo>();

    // ---------- CustomEditorSystem hook ----------
    private Type _customEditorSystemType;
    private MethodInfo _customInit;
    private MethodInfo _customDispose;

    [MenuItem("Tools/Lop Toolbox")]
    public static void Open()
    {
        var w = GetWindow<LopToolboxWindow>("Lop Toolbox");
        w.minSize = new Vector2(520, 360);
        w.Show();
    }

    private void OnEnable()
    {
        titleContent = new GUIContent("Lop Toolbox");
        _so = new SerializedObject(this);

        TryWireCustomEditorSystem();
    }

    private void OnDisable()
    {
        // Try call CustomEditorSystem dispose
        if (_customDispose != null)
        {
            try { _customDispose.Invoke(null, null); }
            catch (Exception e) { Debug.LogWarning($"CustomEditorSystem.Dispose() failed: {e.Message}"); }
        }
    }

    private void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            var newTab = (Tab)GUILayout.Toolbar((int)_tab, Enum.GetNames(typeof(Tab)), EditorStyles.toolbarButton);
            if (newTab != _tab) _tab = newTab;

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Ping Selected", EditorStyles.toolbarButton))
                EditorGUIUtility.PingObject(Selection.activeObject);
        }

        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        switch (_tab)
        {
            case Tab.Effects: DrawEffectsTab(); break;
            case Tab.CirclePaint: DrawCirclePaintTab(); break;
            case Tab.PlayTest: DrawPlayTestTab(); break;
        }
        EditorGUILayout.EndScrollView();
    }

    // =========================
    // Effects Tab
    // =========================
    private void DrawEffectsTab()
    {
        EditorGUILayout.HelpBox("EffectUIList가 있으면 자동으로 공개 메서드를 감지해 버튼을 만들어줘요. 없으면 아래 'Fallback'으로 간단 프리뷰(Instantiate) 기능을 제공합니다.", MessageType.Info);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            _effectTarget = EditorGUILayout.ObjectField(new GUIContent("Effects Target (Component or Asset)"),
                                                        _effectTarget, typeof(UnityEngine.Object), true);
            _effectSpawnParent = (Transform)EditorGUILayout.ObjectField(new GUIContent("Spawn Parent (optional)"), _effectSpawnParent, typeof(Transform), true);
            _effectFilter = EditorGUILayout.TextField(new GUIContent("Method Filter"), _effectFilter);

            if (GUILayout.Button("Scan Methods"))
            {
                ScanEffectMethods();
            }

            if (_effectMethods.Length == 0)
            {
                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Fallback Preview", EditorStyles.boldLabel);
                _fallbackEffectPrefab = (GameObject)EditorGUILayout.ObjectField("Effect Prefab", _fallbackEffectPrefab, typeof(GameObject), false);
                _effectPreviewDuration = EditorGUILayout.Slider("Preview Duration (s)", _effectPreviewDuration, 0.1f, 10f);
                if (GUILayout.Button("Preview Instantiate (SceneView)"))
                {
                    PreviewFallbackEffect();
                }
                return;
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Detected Methods", EditorStyles.boldLabel);
            foreach (var (name, mi) in _effectMethodNames.Zip(_effectMethods, (n, m) => (n, m)))
            {
                if (!string.IsNullOrWhiteSpace(_effectFilter) && !name.ToLower().Contains(_effectFilter.ToLower()))
                    continue;

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(name);
                    if (GUILayout.Button("Invoke", GUILayout.Width(80)))
                    {
                        SafeInvokeEffectMethod(mi);
                    }
                }
            }
        }
    }

    private void ScanEffectMethods()
    {
        _effectMethodNames = Array.Empty<string>();
        _effectMethods = Array.Empty<MethodInfo>();

        if (_effectTarget == null) return;

        var t = _effectTarget.GetType();
        // If it's a GameObject, scan its components
        List<MethodInfo> methods = new List<MethodInfo>();
        List<string> names = new List<string>();

        void collect(Type type, object instance)
        {
            // public instance methods without parameters (common in effect triggers)
            foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (m.GetParameters().Length == 0 && m.ReturnType == typeof(void))
                {
                    // Heuristics: likely effect triggers
                    if (m.Name.StartsWith("Play", StringComparison.OrdinalIgnoreCase) ||
                        m.Name.StartsWith("Show", StringComparison.OrdinalIgnoreCase) ||
                        m.Name.StartsWith("Spawn", StringComparison.OrdinalIgnoreCase) ||
                        m.Name.StartsWith("Stop", StringComparison.OrdinalIgnoreCase))
                    {
                        methods.Add(m);
                        names.Add($"{type.Name}.{m.Name}");
                    }
                }
            }
        }

        if (_effectTarget is GameObject go)
        {
            foreach (var c in go.GetComponents<Component>())
            {
                if (c == null) continue;
                collect(c.GetType(), c);
            }
        }
        else
        {
            collect(t, _effectTarget);
        }

        _effectMethodNames = names.ToArray();
        _effectMethods = methods.ToArray();

        if (_effectMethods.Length == 0)
            EditorUtility.DisplayDialog("No Methods", "호출 가능한 공개 무파라미터 메서드를 찾지 못했어요.\n(예: Play*, Show*, Spawn*, Stop*)", "확인");
    }

    private void SafeInvokeEffectMethod(MethodInfo m)
    {
        try
        {
            object target = _effectTarget;
            if (target is GameObject go)
            {
                // prefer the first component that declares the method
                target = go.GetComponents<Component>().FirstOrDefault(c => c != null && c.GetType() == m.DeclaringType);
            }
            m.Invoke(target, null);
        }
        catch (Exception e)
        {
            Debug.LogError($"Invoke failed: {m.Name}\n{e}");
        }
    }

    private void PreviewFallbackEffect()
    {
        if (_fallbackEffectPrefab == null)
        {
            EditorUtility.DisplayDialog("No Prefab", "Effect Prefab을 지정해 주세요.", "확인");
            return;
        }

        var pos = SceneView.lastActiveSceneView != null
            ? SceneView.lastActiveSceneView.pivot
            : Vector3.zero;

        var go = (GameObject)PrefabUtility.InstantiatePrefab(_fallbackEffectPrefab);
        if (_effectSpawnParent != null) go.transform.SetParent(_effectSpawnParent, true);
        go.transform.position = pos;

        // auto destroy in edit mode after duration
        EditorApplication.delayCall += () => DestroyImmediate(go);
        // Keep it around for a bit
        EditorApplication.delayCall += () =>
        {
            var t = EditorApplication.timeSinceStartup;
            EditorApplication.update += AutoDestruct;

            void AutoDestruct()
            {
                if (EditorApplication.timeSinceStartup - t >= _effectPreviewDuration)
                {
                    if (go != null) DestroyImmediate(go);
                    EditorApplication.update -= AutoDestruct;
                }
            }
        };
    }

    // =========================
    // Circle Paint Tab
    // =========================
    private void DrawCirclePaintTab()
    {
        EditorGUILayout.HelpBox("원형으로 프리팹을 배치합니다. EditorCircleView가 있으면 Gizmo/프리뷰에 더해줄 수 있어요(자동 감지).", MessageType.Info);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            _paintPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _paintPrefab, typeof(GameObject), false);
            _paintCenter = (Transform)EditorGUILayout.ObjectField("Center Transform", _paintCenter, typeof(Transform), true);

            _radius = EditorGUILayout.Slider("Radius", _radius, 0.1f, 50f);
            _countOnCircle = EditorGUILayout.IntSlider("Points", _countOnCircle, 1, 256);

            _snapToGround = EditorGUILayout.Toggle("Snap To Ground", _snapToGround);
            _groundMask = LayerMaskField("Ground Mask", _groundMask);

            _seed = EditorGUILayout.IntField("Random Seed", _seed);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Preview Gizmo")) SceneView.RepaintAll();
                if (GUILayout.Button("Place"))
                {
                    PlaceCircleInstances();
                }
            }
        }

        // Draw gizmo preview in SceneView
        SceneView.duringSceneGui -= OnSceneGUI_CirclePreview;
        SceneView.duringSceneGui += OnSceneGUI_CirclePreview;
    }

    private void OnSceneGUI_CirclePreview(SceneView view)
    {
        if (_paintCenter == null) return;

        Handles.color = new Color(0.2f, 0.8f, 1f, 0.75f);
        Handles.DrawWireDisc(_paintCenter.position, Vector3.up, _radius);

        // points
        using (new Handles.DrawingScope(Color.white))
        {
            for (int i = 0; i < Mathf.Max(1, _countOnCircle); i++)
            {
                float t = (i / (float)_countOnCircle) * Mathf.PI * 2f;
                var p = _paintCenter.position + new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * _radius;
                Handles.SphereHandleCap(0, p, Quaternion.identity, HandleUtility.GetHandleSize(p) * 0.05f, EventType.Repaint);
            }
        }
    }

    private void PlaceCircleInstances()
    {
        if (_paintPrefab == null || _paintCenter == null)
        {
            EditorUtility.DisplayDialog("Need Prefab/Center", "Prefab과 Center Transform을 지정해 주세요.", "확인");
            return;
        }

        var parent = _paintCenter;
        var count = Mathf.Max(1, _countOnCircle);
        var rand = new System.Random(_seed);

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();

        for (int i = 0; i < count; i++)
        {
            float t = (i / (float)count) * Mathf.PI * 2f;
            var pos = _paintCenter.position + new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * _radius;

            if (_snapToGround)
            {
                if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out var hit, 200f, _groundMask))
                    pos = hit.point;
            }

            var inst = (GameObject)PrefabUtility.InstantiatePrefab(_paintPrefab);
            Undo.RegisterCreatedObjectUndo(inst, "Place Circle Prefab");
            inst.transform.SetPositionAndRotation(pos, Quaternion.Euler(0f, rand.Next(0, 360), 0f));
            inst.transform.SetParent(parent, true);
        }

        Undo.CollapseUndoOperations(group);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    // LayerMask field drawer
    private static LayerMask LayerMaskField(string label, LayerMask selected)
    {
        var layers = Enumerable.Range(0, 32).Select(i => LayerMask.LayerToName(i)).ToArray();
        var layerNumbers = Enumerable.Range(0, 32).ToArray();

        var selectedMask = 0;
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & selected.value) != 0)
                selectedMask |= (1 << i);
        }
        selectedMask = EditorGUILayout.MaskField(label, selectedMask, layers);
        selected.value = selectedMask;
        return selected;
    }

    // =========================
    // PlayTest Tab
    // =========================
    private void DrawPlayTestTab()
    {
        EditorGUILayout.HelpBox("PlayTest 컴포넌트의 공개 무파라미터 메서드를 자동으로 나열합니다. 클릭하면 그 함수 호출!", MessageType.Info);

        using (new EditorGUILayout.VerticalScope("box"))
        {
            _playTestTarget = EditorGUILayout.ObjectField("PlayTest Target (GameObject/Script)", _playTestTarget, typeof(UnityEngine.Object), true);

            if (GUILayout.Button("Scan Methods")) ScanPlayTestMethods();

            if (_playTestCallables.Length == 0)
            {
                EditorGUILayout.LabelField("메서드를 찾지 못했어요. 공개(void, 파라미터 없음) 메서드를 구현해 두면 목록에 뜹니다.");
                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Enter PlayMode")) EditorApplication.isPlaying = true;
                    if (GUILayout.Button("Exit PlayMode")) EditorApplication.isPlaying = false;
                }
                return;
            }

            EditorGUILayout.Space(4);
            foreach (var (name, mi) in _playTestCallableNames.Zip(_playTestCallables, (n, m) => (n, m)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(name);
                    if (GUILayout.Button("Run", GUILayout.Width(80)))
                    {
                        SafeInvokePlayTestMethod(mi);
                    }
                }
            }

            EditorGUILayout.Space(8);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Enter PlayMode")) EditorApplication.isPlaying = true;
                if (GUILayout.Button("Exit PlayMode")) EditorApplication.isPlaying = false;
            }
        }
    }

    private void ScanPlayTestMethods()
    {
        _playTestCallableNames = Array.Empty<string>();
        _playTestCallables = Array.Empty<MethodInfo>();

        if (_playTestTarget == null) return;

        var t = _playTestTarget.GetType();
        var methods = new List<MethodInfo>();
        var names = new List<string>();

        void collect(Type type, object instance)
        {
            foreach (var m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (m.GetParameters().Length == 0 && m.ReturnType == typeof(void))
                {
                    names.Add($"{type.Name}.{m.Name}");
                    methods.Add(m);
                }
            }
        }

        if (_playTestTarget is GameObject go)
        {
            foreach (var c in go.GetComponents<Component>())
            {
                if (c == null) continue;
                collect(c.GetType(), c);
            }
        }
        else
        {
            collect(t, _playTestTarget);
        }

        _playTestCallableNames = names.ToArray();
        _playTestCallables = methods.ToArray();
    }

    private void SafeInvokePlayTestMethod(MethodInfo m)
    {
        try
        {
            object target = _playTestTarget;
            if (target is GameObject go)
                target = go.GetComponents<Component>().FirstOrDefault(c => c != null && c.GetType() == m.DeclaringType);

            if (!EditorApplication.isPlaying)
            {
                if (!EditorUtility.DisplayDialog("Not in Play Mode",
                        "플레이 모드가 아닙니다. 에디터 모드에서 실행하면 의도치 않은 상태일 수 있어요. 그래도 실행할까요?",
                        "실행", "취소")) return;
            }

            m.Invoke(target, null);
        }
        catch (Exception e)
        {
            Debug.LogError($"PlayTest invoke failed: {m.Name}\n{e}");
        }
    }

    // =========================
    // CustomEditorSystem Hook
    // =========================
    private void TryWireCustomEditorSystem()
    {
        _customEditorSystemType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name.Equals("CustomEditorSystem", StringComparison.Ordinal));

        if (_customEditorSystemType == null) return;

        _customInit = _customEditorSystemType.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static);
        _customDispose = _customEditorSystemType.GetMethod("Dispose", BindingFlags.Public | BindingFlags.Static);

        if (_customInit != null)
        {
            try { _customInit.Invoke(null, null); }
            catch (Exception e) { Debug.LogWarning($"CustomEditorSystem.Initialize() failed: {e.Message}"); }
        }
    }
}
#endif
