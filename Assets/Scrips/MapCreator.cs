using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class NoteData
{
    public float time;       // 노트가 나오는 시간 (초 단위)
    public string type;      // R, G, B, RG 등
}

[System.Serializable]
public class GimmickData
{
    public float time;       // 기믹 발생 시간
    public string type;      // SpeedUp, FadeOut 등
    public float value;      // 기믹 강도 또는 배속 등
    public float duration;   // 지속 시간 (필요한 경우만)
}

[System.Serializable]
public class MapData
{
    public string songName;          // 곡 이름
    public float bpm;                // BPM
    public List<NoteData> notes;     // 노트 리스트
    public List<GimmickData> gimmicks; // 기믹 리스트
}

public class MapCreator : MonoBehaviour
{
    void Start()
    {
        // 예시 맵 데이터 생성
        MapData map = new MapData
        {
            songName = "test_track",
            bpm = 120f,
            notes = new List<NoteData>
            {
                new NoteData { time = 1.2f, type = "R" },
                new NoteData { time = 2.3f, type = "G" },
                new NoteData { time = 3.5f, type = "B" },
                new NoteData { time = 4.0f, type = "RG" }
            },
            gimmicks = new List<GimmickData>
            {
                new GimmickData { time = 5.0f, type = "SpeedUp", value = 1.5f, duration = 0f },
                new GimmickData { time = 7.2f, type = "FadeOut", value = 0f, duration = 2.0f }
            }
        };

        // JSON 문자열 생성
        string json = JsonUtility.ToJson(map, true);

        // 저장 경로: Assets/Resources/Maps/
        string directory = Application.dataPath + "/Resources/Maps";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string path = Path.Combine(directory, "test_track.json");

        File.WriteAllText(path, json);

        Debug.Log("맵 JSON 저장 완료: " + path);
    }
}
