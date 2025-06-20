using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteRecorder : MonoBehaviour
{
    [Header("설정")]
    public AudioSource music;
    public float timingOffset = -0.08f;
    public float bpm = 120f;
    public bool useSnap = true;
    public KeyCode recordKey = KeyCode.Space;
    public KeyCode saveKey = KeyCode.S;

    private List<Note> notes = new();

    void Update()
    {
        if (music.isPlaying && Input.GetKeyDown(recordKey))
        {
            float time = music.time + timingOffset;
            float finalTime = useSnap ? SnapToBeat(time) : time;

            string noteType = GetRandomNoteType();
            notes.Add(new Note { time = Mathf.Round(finalTime * 1000f) / 1000f, type = noteType });
            Debug.Log($"노트 입력: {finalTime:F3} ({noteType})");
        }

        if (Input.GetKeyDown(saveKey))
        {
            SaveNotesToJson();
        }
    }

    float SnapToBeat(float time)
    {
        float beat = 60f / bpm / 2f; // 8분음표
        return Mathf.Round(time / beat) * beat;
    }

    string GetRandomNoteType()
    {
        float rand = Random.value;

        if (rand < 0.5f)
        {
            string[] singles = { "R", "G", "B" };
            return singles[Random.Range(0, singles.Length)];
        }
        else if (rand < 0.8f)
        {
            string[] doubles = { "RG", "RB", "GB" };
            return doubles[Random.Range(0, doubles.Length)];
        }
        else
        {
            return "RGB";
        }
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
        public string type;
    }

    [System.Serializable]
    public class NoteDataWrapper
    {
        public Note[] notes;
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (notes == null) return;

        Gizmos.color = Color.red;

        foreach (var note in notes)
        {
            // 시간 값 기준으로 Y축에 띄워서 표현
            Vector3 pos = transform.position + new Vector3(0f, note.time * 2f, 0f);

            switch (note.type)
            {
                case "R": Gizmos.color = Color.red; break;
                case "G": Gizmos.color = Color.green; break;
                case "B": Gizmos.color = Color.blue; break;
                case "RG": Gizmos.color = Color.yellow; break;
                case "RB": Gizmos.color = new Color(1f, 0f, 1f); break; // 핑크
                case "GB": Gizmos.color = Color.cyan; break;
                case "RGB": Gizmos.color = Color.white; break;
            }

            Gizmos.DrawSphere(pos, 0.2f);
        }
    }
#endif

}
