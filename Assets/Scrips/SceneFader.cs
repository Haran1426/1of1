using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    // ===== Singleton (필요 시 자동 생성) =====
    private static SceneFader _instance;
    public static SceneFader Instance
    {
        get
        {
            if (_instance != null) return _instance;

            var go = new GameObject("[SceneFader]");
            _instance = go.AddComponent<SceneFader>();
            DontDestroyOnLoad(go);
            _instance.BuildOverlay();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 자기 자신만 유지
            BuildOverlay();
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    // ===== Settings =====
    [SerializeField] private Color defaultFadeColor = Color.black;
    [SerializeField] private int sortingOrder = 9999; // 항상 최상단
    [SerializeField] private bool blockRaycastDuringFade = true;

    private Image overlay;   // 전체 화면을 덮는 Image
    private bool isBusy;

    // 최초 생성 시 오버레이 구성
    private void BuildOverlay()
    {
        // Canvas
        var canvasGO = new GameObject("FaderCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);

        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Fullscreen Image
        var imgGO = new GameObject("Overlay", typeof(Image));
        imgGO.transform.SetParent(canvasGO.transform, false);
        overlay = imgGO.GetComponent<Image>();
        overlay.color = new Color(defaultFadeColor.r, defaultFadeColor.g, defaultFadeColor.b, 0f);
        overlay.raycastTarget = false;

        // 🔥 화면 전체 덮게 만들기 대신 고정 크기
        var rt = overlay.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);  // 가운데 정렬
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(Screen.width, 2000); // 가로는 화면 너비, 세로는 1000 고정

    }


    // ===== Public API (어디서든 호출) =====

    /// <summary>
    /// 씬을 페이드 아웃→로드→페이드 인
    /// </summary>
    public static void FadeToScene(string sceneName, float fadeOut = 0.35f, float fadeIn = 0.35f, Color? color = null, LoadSceneMode mode = LoadSceneMode.Single)
    {
        Instance.StartCoroutine(Instance.Co_FadeToScene(sceneName, fadeOut, fadeIn, color ?? Instance.defaultFadeColor, mode));
    }

    /// <summary>
    /// 단순 페이드 아웃(검정으로 덮기)
    /// </summary>
    public static void FadeOut(float duration = 0.35f, Color? color = null)
    {
        Instance.StartCoroutine(Instance.Co_Fade(0f, 1f, duration, color ?? Instance.defaultFadeColor));
    }

    /// <summary>
    /// 단순 페이드 인(밝아지기)
    /// </summary>
    public static void FadeIn(float duration = 0.35f, Color? color = null)
    {
        Instance.StartCoroutine(Instance.Co_Fade(1f, 0f, duration, color ?? Instance.defaultFadeColor));
    }

    // ===== Coroutines =====

    private IEnumerator Co_FadeToScene(string sceneName, float outDur, float inDur, Color color, LoadSceneMode mode)
    {
        if (isBusy) yield break;
        isBusy = true;

        yield return Co_Fade(0f, 1f, outDur, color);               // 어두워지기
        yield return SceneManager.LoadSceneAsync(sceneName, mode);  // 로드
        yield return null;                                         // 한 프레임 대기(안전)
        yield return Co_Fade(1f, 0f, inDur, color);                 // 밝아지기

        isBusy = false;
    }

    private IEnumerator Co_Fade(float from, float to, float duration, Color color)
    {
        if (duration <= 0f)
        {
            color.a = to;
            overlay.color = color;
            overlay.raycastTarget = blockRaycastDuringFade && (to > 0.01f);
            yield break;
        }

        overlay.raycastTarget = blockRaycastDuringFade; // 클릭 막기
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / duration);
            // 부드러운 S-curve (SmoothStep)
            u = u * u * (3f - 2f * u);
            float a = Mathf.Lerp(from, to, u);
            var c = color; c.a = a;
            overlay.color = c;
            yield return null;
        }
        var final = color; final.a = to;
        overlay.color = final;
        overlay.raycastTarget = blockRaycastDuringFade && (to > 0.01f);
    }
}
