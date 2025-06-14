using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class AutoNoteRecorder : MonoBehaviour
{
    [Header("Audio & Timing")]
    public AudioSource audioSource;
    public float bpm = 120f;
    [Range(-0.3f, 0.3f)]
    public float timingOffset = 0.1f;

    [Header("Detection Settings")]
    public float threshold = 0.5f;       
    public float minInterval = 0.3f;    

    [Header("Controls")]
    public KeyCode stopKey = KeyCode.Return;

    private float lastNoteTime = 0f;
    private List<Note> notes = new();
    private bool isRecording = true;

    float[] samples = new float[1024];

    void Update()
    {
        if (!audioSource.isPlaying || !isRecording) return;

        if (Input.GetKeyDown(stopKey))
        {
            isRecording = false;
            SaveToJson();
            Debug.Log("🎵 녹음 종료됨. 'Go_On.json'으로 저장됨.");
            return;
        }

        audioSource.GetOutputData(samples, 0);
        float volume = 0f;
        foreach (float s in samples) volume += Mathf.Abs(s);
        volume /= samples.Length;

        if (volume > threshold && Time.time - lastNoteTime > minInterval)
        {
            lastNoteTime = Time.time;

            float snappedTime = SnapToBeat(audioSource.time + timingOffset, bpm);
            string noteType = GetNoteType(volume);

            notes.Add(new Note
            {
                time = Mathf.Round(snappedTime * 1000f) / 1000f,
                type = noteType
            });

            Debug.Log($"\"time\": {snappedTime:F2}, \"type\": \"{noteType}\", volume: {volume:F3}");
        }
    }

    string GetNoteType(float volume)
    {
        float chance = Random.value;

        if (volume >= 0.3f)
        {
            if (chance < 0.4f) return "RGB"; // 40%
            else if (chance < 0.8f)
            {
                string[] mids = { "RG", "RB", "GB" };
                return mids[Random.Range(0, mids.Length)];
            }
            else
            {
                string[] lows = { "R", "G", "B" };
                return lows[Random.Range(0, lows.Length)];
            }
        }
        else if (volume >= 0.15f)
        {
            if (chance < 0.2f) return "RGB"; // 20%
            else if (chance < 0.6f)
            {
                string[] mids = { "RG", "RB", "GB" };
                return mids[Random.Range(0, mids.Length)];
            }
            else
            {
                string[] lows = { "R", "G", "B" };
                return lows[Random.Range(0, lows.Length)];
            }
        }
        else
        {
            string[] lows = { "R", "G", "B" };
            return lows[Random.Range(0, lows.Length)];
        }
    }

    float SnapToBeat(float time, float bpm)
    {
        float beatDuration = 60f / bpm;
        return Mathf.Round(time / beatDuration) * beatDuration;
    }

    void SaveToJson()
    {
        var mapData = new MapData
        {
            songName = "Go_On",
            bpm = bpm,
            notes = notes,
            gimmicks = new List<Gimmick>() // 비워둠
        };

        string json = JsonUtility.ToJson(mapData, true);

        string folderPath = Path.Combine(Application.dataPath, "Resources", "Maps");
        string filePath = Path.Combine(folderPath, "Go_On.json");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        File.WriteAllText(filePath, json);
        Debug.Log($"✅ 노트 저장 완료: {filePath}");
    }

    [System.Serializable]
    public class Note
    {
        public float time;
        public string type;
    }

    [System.Serializable]
    public class Gimmick
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
        public List<Note> notes;
        public List<Gimmick> gimmicks;
    }
}
