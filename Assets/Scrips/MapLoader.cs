using UnityEngine;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
    [System.Serializable]
    public class NoteData
    {
        public float time;
        public string type;
    }

    [System.Serializable]
    public class GimmickData
    {
        public float time;
        public string type;
        public float value;
        public float duration;
    }

    [System.Serializable]
    public class MapData
    {
        public string songName;
        public float bpm;
        public List<NoteData> notes;
        public List<GimmickData> gimmicks;
    }

    public string mapName = "test_track"; // Resources/Maps/test_track.json

    void Start()
    {
        LoadMap(mapName);
    }

    void LoadMap(string name)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Maps/{name}");
        if (jsonFile == null)
        {
            Debug.LogError("맵 파일을 찾을 수 없습니다: " + name);
            return;
        }

        MapData map = JsonUtility.FromJson<MapData>(jsonFile.text);

        Debug.Log("맵 불러오기 성공: " + map.songName);
        Debug.Log("BPM: " + map.bpm);
        Debug.Log("노트 개수: " + map.notes.Count);
        Debug.Log("기믹 개수: " + map.gimmicks.Count);

        foreach (var note in map.notes)
        {
            Debug.Log($"노트 - 시간: {note.time}, 타입: {note.type}");
        }

        foreach (var gimmick in map.gimmicks)
        {
            Debug.Log($"기믹 - 시간: {gimmick.time}, 타입: {gimmick.type}, 값: {gimmick.value}, 지속: {gimmick.duration}");
        }
    }
}
