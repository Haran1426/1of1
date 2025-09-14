using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Data_Manager : MonoBehaviour
{
    public MapData mapData = new MapData();
    public GameObject[] notePrefabList;

    [Header("저장 파일 이름")]
    public string saveFileName = "MapData.json";

    [Header("BPM/속도 설정")]
    public float BPM = 120f;
    public float scrollSpeed = 5f;

    // ===== Undo/Redo용 구조체와 스택 =====
    private struct PlacedNote
    {
        public NoteData data;
        public GameObject obj; // 씬에 존재하는 인스턴스 (Undo시 삭제, Redo시 재생성)
    }

    private Stack<PlacedNote> undoStack = new Stack<PlacedNote>();
    private Stack<PlacedNote> redoStack = new Stack<PlacedNote>();

    private void Awake()
    {
        if (mapData.notes == null)
            mapData.notes = new List<NoteData>();
    }

    private void Update()
    {
        HandleUndoRedoShortcuts();
    }

    private void HandleUndoRedoShortcuts()
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
                    || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand); // mac도 지원
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (ctrl && Input.GetKeyDown(KeyCode.Z) && !shift)
        {
            Undo();
        }
        else if (ctrl && (Input.GetKeyDown(KeyCode.Y) || (Input.GetKeyDown(KeyCode.Z) && shift)))
        {
            // Ctrl+Y 또는 Ctrl+Shift+Z
            Redo();
        }
    }

    public void AddNote(int type, float x, float y)
    {
        NoteData data = new NoteData { type = type, posX = x, posY = y };

        // 거리 / 박자 계산
        if (mapData.notes.Count > 0)
        {
            NoteData prev = mapData.notes[mapData.notes.Count - 1];
            data.distance = Mathf.Abs(x - prev.posX);

            float deltaTime = data.distance / scrollSpeed;
            data.beat = deltaTime * (BPM / 60f);
        }
        else
        {
            data.distance = 0;
            data.beat = 0;
        }

        // 데이터 먼저 기록
        mapData.notes.Add(data);

        // 인스턴스 생성
        GameObject obj = null;
        if (type > 0 && type <= notePrefabList.Length)
        {
            obj = Instantiate(notePrefabList[type - 1], new Vector3(x, y, 0), Quaternion.identity);
            obj.tag = "Note";
        }

        // Undo 스택에 액션 푸시, Redo 스택 초기화
        undoStack.Push(new PlacedNote { data = data, obj = obj });
        redoStack.Clear();
    }

    public bool ExistsNoteAtPosition(Vector3 pos)
    {
        foreach (var note in mapData.notes)
        {
            if (Mathf.Approximately(note.posX, pos.x) &&
                Mathf.Approximately(note.posY, pos.y))
                return true;
        }
        return false;
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;

        var placed = undoStack.Pop();

        // 데이터 제거 (안전하게 뒤에서부터 탐색)
        for (int i = mapData.notes.Count - 1; i >= 0; i--)
        {
            var n = mapData.notes[i];
            if (Mathf.Approximately(n.posX, placed.data.posX) &&
                Mathf.Approximately(n.posY, placed.data.posY) &&
                n.type == placed.data.type)
            {
                mapData.notes.RemoveAt(i);
                break;
            }
        }

        // 인스턴스 제거
        if (placed.obj != null) Destroy(placed.obj);

        // Redo 스택에 저장
        redoStack.Push(placed);

        Debug.Log("Undo 실행됨");
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;

        var placed = redoStack.Pop();

        // 데이터 복구
        mapData.notes.Add(placed.data);

        // 인스턴스 복구
        GameObject obj = null;
        if (placed.data.type > 0 && placed.data.type <= notePrefabList.Length)
        {
            obj = Instantiate(notePrefabList[placed.data.type - 1],
                new Vector3(placed.data.posX, placed.data.posY, 0), Quaternion.identity);
            obj.tag = "Note";
        }

        // 최신 인스턴스 참조로 교체하여 Undo 스택에 저장
        undoStack.Push(new PlacedNote { data = placed.data, obj = obj });

        Debug.Log("Redo 실행됨");
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(mapData, true);
        string path = Path.Combine(Application.dataPath, "Resources/Maps", saveFileName);
        File.WriteAllText(path, json);
        Debug.Log("노트 데이터 저장 완료: " + path);
    }

    public void LoadFromJson()
    {
        string path = Path.Combine(Application.dataPath, "Resources/Maps", saveFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            mapData = JsonUtility.FromJson<MapData>(json);

            Debug.Log("노트 데이터 불러오기 완료: " + mapData.notes.Count + "개");
        }
        else
        {
            Debug.LogWarning("저장된 맵 파일이 없습니다: " + path);
        }
    }

    public void SpawnNotesFromData()
    {
        foreach (var note in mapData.notes)
        {
            if (note.type > 0 && note.type <= notePrefabList.Length)
            {
                GameObject obj = Instantiate(notePrefabList[note.type - 1],
                    new Vector3(note.posX, note.posY, 0), Quaternion.identity);
                obj.tag = "Note";
            }
        }
    }
}
