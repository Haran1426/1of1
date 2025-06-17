using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteRecorder : MonoBehaviour
{
    [Header("설정")]
    public AudioSource music;              // 음악 재생용
    public float timingOffset = -0.08f;    // 입력 보정 시간
    public float bpm = 120f;               // BPM
    public bool useSnap = true;            // 비트 스냅 여부
    public KeyCode recordKey = KeyCode.Space;  // 노트 입력 키
    public KeyCode saveKey = KeyCode.S;        // 저장 키

    private List<Note> notes = new();      // 노트 목록

    void Update()
    {
        if (music.isPlaying && Input.GetKeyDown(recordKey))
        {
            float time = Time.time + timingOffset;
            float finalTime = useSnap ? SnapToBeat(time) : time;

            notes.Add(new Note { time = Mathf.Round(finalTime * 1000f) / 1000f, type = "Red" }); // 기본 노트는 Red
            Debug.Log($"노트 입력: {finalTime:F3}");
        }

        if (Input.GetKeyDown(saveKey))
        {
            SaveNotesToJson();
        }
    }

    float SnapToBeat(float time)
    {
        float beat = 60f / bpm / 2f; // 8분음표 기준
        return Mathf.Round(time / beat) * beat;
    }

    void SaveNotesToJson()
    {
        NoteDataWrapper wrapper = new() { notes = notes.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);
        string path = Application.dataPath + "/noteData.json";
        File.WriteAllText(path, json);
        Debug.Log($"🎵 저장 완료: {path}");
    }

    [System.Serializable]
    public class Note
    {
        public float time;
        public string type; // 예: Red, RG, RGB 등
    }

    [System.Serializable]
    public class NoteDataWrapper
    {
        public Note[] notes;
    }
}
