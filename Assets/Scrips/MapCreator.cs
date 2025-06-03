using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MapCreator : MonoBehaviour
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

    void Start()
    {
        MapData map = new MapData
        {
            songName = "test_track",
            bpm = 120f,
            notes = new List<NoteData>
            {
                new NoteData { time = 1.0f, type = "R" },
                new NoteData { time = 2.0f, type = "G" },
                new NoteData { time = 3.0f, type = "B" },
                new NoteData { time = 4.0f, type = "RG" },
                new NoteData { time = 5.0f, type = "RB" },
                new NoteData { time = 6.0f, type = "GB" },
                new NoteData { time = 7.0f, type = "RGB" }
            },
            gimmicks = new List<GimmickData>
            {
                new GimmickData { time = 8.0f, type = "SpeedUp", value = 1.5f, duration = 0f },
                new GimmickData { time = 10.0f, type = "FadeOut", value = 0f, duration = 2.0f }
            }
        };

        string json = JsonUtility.ToJson(map, true);

        string directory = Application.dataPath + "/Resources/Maps";
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string path = Path.Combine(directory, "test_track.json");
        File.WriteAllText(path, json);

        Debug.Log("¸Ê »ý¼º ¿Ï·á: " + path);
    }
}
