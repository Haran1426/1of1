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
            Debug.LogError("�� ������ ã�� �� �����ϴ�: " + name);
            return;
        }

        MapData map = JsonUtility.FromJson<MapData>(jsonFile.text);

        Debug.Log("�� �ҷ����� ����: " + map.songName);
        Debug.Log("BPM: " + map.bpm);
        Debug.Log("��Ʈ ����: " + map.notes.Count);
        Debug.Log("��� ����: " + map.gimmicks.Count);

        foreach (var note in map.notes)
        {
            Debug.Log($"��Ʈ - �ð�: {note.time}, Ÿ��: {note.type}");
        }

        foreach (var gimmick in map.gimmicks)
        {
            Debug.Log($"��� - �ð�: {gimmick.time}, Ÿ��: {gimmick.type}, ��: {gimmick.value}, ����: {gimmick.duration}");
        }
    }
}
