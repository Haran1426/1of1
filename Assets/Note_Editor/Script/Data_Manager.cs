using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class NoteData
{
    public int type;
    public float posX;
    public float posY;

    public float distance; // 이전 노트와의 거리
    public float beat;     // 이전 노트와의 박자 차이
}

[System.Serializable]
public class NoteDataList
{
    public List<NoteData> notes = new List<NoteData>();
}

public class Data_Manager : MonoBehaviour
{
    public NoteDataList noteDataList = new NoteDataList();
    public GameObject[] notePrefabList;

    [Header("저장 파일 이름")]
    public string saveFileName = "MapData.json";

    [Header("BPM/속도 설정")]
    public float BPM = 120f;          // 곡 BPM
    public float scrollSpeed = 5f;    // 노트 스크롤 속도 (유닛/초)

    // 🔥 Undo / Redo 스택
    private Stack<NoteData> undoStack = new Stack<NoteData>();
    private Stack<NoteData> redoStack = new Stack<NoteData>();

    /// <summary>
    /// 노트 추가 & 프리팹 배치
    /// </summary>
    public void AddNote(int type, float x, float y)
    {
        NoteData data = new NoteData { type = type, posX = x, posY = y };

        // ✅ 거리/박자 계산
        if (noteDataList.notes.Count > 0)
        {
            NoteData prev = noteDataList.notes[noteDataList.notes.Count - 1];
            data.distance = Mathf.Abs(x - prev.posX);

            float deltaTime = data.distance / scrollSpeed; // 초 단위 시간
            data.beat = deltaTime * (BPM / 60f);           // 박자 단위
        }
        else
        {
            data.distance = 0;
            data.beat = 0;
        }

        noteDataList.notes.Add(data);
        undoStack.Push(data);
        redoStack.Clear();

        if (type > 0 && type <= notePrefabList.Length)
        {
            GameObject obj = Instantiate(notePrefabList[type - 1], new Vector3(x, y, 0), Quaternion.identity);
            obj.tag = "Note";
        }
        else
        {
            Debug.LogError($"잘못된 노트 타입: {type}, 프리팹 리스트 길이: {notePrefabList.Length}");
        }
    }

    /// <summary>
    /// 같은 좌표에 노트가 있는지 검사
    /// </summary>
    public bool ExistsNoteAtPosition(Vector3 pos)
    {
        foreach (var note in noteDataList.notes)
        {
            if (Mathf.Approximately(note.posX, pos.x) &&
                Mathf.Approximately(note.posY, pos.y))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Undo: 최근 노트 삭제
    /// </summary>
    public void Undo()
    {
        if (noteDataList.notes.Count == 0) return;

        NoteData last = noteDataList.notes[noteDataList.notes.Count - 1];
        noteDataList.notes.RemoveAt(noteDataList.notes.Count - 1);
        redoStack.Push(last);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Note"))
        {
            if (Mathf.Approximately(obj.transform.position.x, last.posX) &&
                Mathf.Approximately(obj.transform.position.y, last.posY))
            {
                Destroy(obj);
                break;
            }
        }

        Debug.Log("Undo 실행됨");
    }

    /// <summary>
    /// Redo: 방금 취소한 노트 다시 배치
    /// </summary>
    public void Redo()
    {
        if (redoStack.Count == 0) return;

        NoteData redoNote = redoStack.Pop();
        noteDataList.notes.Add(redoNote);
        undoStack.Push(redoNote);

        if (redoNote.type > 0 && redoNote.type <= notePrefabList.Length)
        {
            GameObject obj = Instantiate(notePrefabList[redoNote.type - 1],
                new Vector3(redoNote.posX, redoNote.posY, 0), Quaternion.identity);
            obj.tag = "Note";
        }

        Debug.Log("Redo 실행됨");
    }

    /// <summary>
    /// JSON 저장
    /// </summary>
    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(noteDataList, true);
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path, json);
        Debug.Log("노트 데이터 저장 완료: " + path);
    }

    /// <summary>
    /// JSON 불러오기
    /// </summary>
    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            noteDataList = JsonUtility.FromJson<NoteDataList>(json);

            Debug.Log("노트 데이터 불러오기 완료: " + noteDataList.notes.Count + "개");
        }
        else
        {
            Debug.LogWarning("저장된 맵 파일이 없습니다: " + path);
        }
    }

    /// <summary>
    /// 불러온 데이터 배치
    /// </summary>
    public void SpawnNotesFromData()
    {
        foreach (var note in noteDataList.notes)
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
