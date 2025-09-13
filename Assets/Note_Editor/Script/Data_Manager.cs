using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class NoteData
{
    public int type;   // 노트 타입 
    public float posX; // 노트 X좌표
    public float posY; // 노트 Y좌표
    public float bpm;  // 해당 노트의 BPM
    public float distance; // 곡 진행 거리(시간 기반 계산용)
}

[System.Serializable]
public class NoteDataList
{
    public List<NoteData> notes = new List<NoteData>();
}

public class Data_Manager : MonoBehaviour
{
    [Header("파일 이름 (확장자 제외)")]
    [SerializeField] private string fileName = "NoteData";

    private string FilePath => Path.Combine(Application.persistentDataPath, fileName + ".json");

    public NoteDataList noteDataList = new NoteDataList();

    // 저장하기
    public void SaveData()
    {
        string json = JsonUtility.ToJson(noteDataList, true);
        File.WriteAllText(FilePath, json);
        Debug.Log($"노트 데이터 저장 완료: {FilePath}");
    }

    //불러오기
    public void LoadData()
    {
        if (!File.Exists(FilePath))
        {
            Debug.LogWarning("저장된 노트 파일 없음");
            return;
        }

        string json = File.ReadAllText(FilePath);
        noteDataList = JsonUtility.FromJson<NoteDataList>(json);
        Debug.Log($"노트 데이터 불러오기 완료: {FilePath}, 개수: {noteDataList.notes.Count}");
    }

    // ✅ DataList에 노트 추가
    public void AddNoteData(int type, Vector3 pos, float bpm, float distance)
    {
        NoteData newNote = new NoteData
        {
            type = type,
            posX = pos.x,
            posY = pos.y,
            bpm = bpm,
            distance = distance
        };

        noteDataList.notes.Add(newNote);
        Debug.Log($"노트 추가: 타입={type}, 위치={pos}");
    }
}
