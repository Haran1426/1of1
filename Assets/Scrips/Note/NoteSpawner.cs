using UnityEngine;
using System.Collections.Generic;
using static MapCreator; // MapCreator 안의 NoteData, GimmickData, MapData 사용

public class NoteSpawner : MonoBehaviour
{
    [Header("Rhythm setting")]
    public AudioSource musicSource;
    public string mapName = "Go_On"; // Resources/Maps/test_track.json

    [Header("Note Settings")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 3f;
    public int maxObjects = 999;

    [Header("Note Sprites")]
    public Sprite redNote, greenNote, blueNote;
    public Sprite yellowNote, magentaNote, cyanNote, whiteNote;

    [Header("HitLine")]
    public Transform HitLine;
    public float judgementRange = 1f;

    public enum NoteType { Red, Green, Blue, Yellow, Magenta, Cyan, White }

    private List<GameObject> spawnedObjects = new();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    private MapData mapData;
    private int currentNoteIndex = 0;

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Maps/{mapName}");
        if (jsonFile == null)
        {
            Debug.LogError("맵을 찾을 수 없습니다: " + mapName);
            return;
        }

        mapData = JsonUtility.FromJson<MapData>(jsonFile.text);
        if (mapData == null || mapData.notes == null)
        {
            Debug.LogError("맵 데이터 파싱 실패");
            return;
        }

        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying || mapData == null) return;

        float currentTime = musicSource.time;

        while (currentNoteIndex < mapData.notes.Count &&
               mapData.notes[currentNoteIndex].time <= currentTime)
        {
            SpawnNote(mapData.notes[currentNoteIndex]);
            currentNoteIndex++;
        }

        HandleInput();
    }

    void SpawnNote(NoteData noteData)
    {
        if (spawnedObjects.Count >= maxObjects) return;

        float y = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPos = new Vector3(spawnX, y, 0);
        GameObject note = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        NoteType type = ConvertType(noteData.type);

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForType(type);

        Note data = note.AddComponent<Note>();
        data.noteType = type;

        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * moveSpeed;

        note.tag = "Note";
        spawnedObjects.Add(note);
    }

    void HandleInput()
    {
        spawnedObjects.RemoveAll(note => note == null);

        if (Input.anyKeyDown)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                GameObject note = spawnedObjects[i];
                if (note == null) continue;

                float distance = Mathf.Abs(note.transform.position.x - HitLine.position.x);
                if (distance > judgementRange) continue;

                Note data = note.GetComponent<Note>();
                if (data == null) continue;

                bool correct = CheckInput(data.noteType);

                if (correct)
                {
                    Debug.Log("노트 히트!");
                    Score.Instance?.OnHitHitBox();

                    Destroy(note);
                    spawnedObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }
    public void RemoveFromList(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
        {
            spawnedObjects.Remove(obj);
        }
    }
    bool CheckInput(NoteType type)
    {
        bool r = Input.GetKey(KeyCode.R);
        bool g = Input.GetKey(KeyCode.G);
        bool b = Input.GetKey(KeyCode.B);

        return type switch
        {
            NoteType.Red => r && !g && !b,
            NoteType.Green => !r && g && !b,
            NoteType.Blue => !r && !g && b,
            NoteType.Yellow => r && g && !b,
            NoteType.Magenta => r && !g && b,
            NoteType.Cyan => !r && g && b,
            NoteType.White => r && g && b,
            _ => false
        };
    }

    NoteType ConvertType(string type)
    {
        return type switch
        {
            "R" => NoteType.Red,
            "G" => NoteType.Green,
            "B" => NoteType.Blue,
            "RG" => NoteType.Yellow,
            "RB" => NoteType.Magenta,
            "GB" => NoteType.Cyan,
            "RGB" => NoteType.White,
            _ => NoteType.Red
        };
    }

    Sprite GetSpriteForType(NoteType type)
    {
        return type switch
        {
            NoteType.Red => redNote,
            NoteType.Green => greenNote,
            NoteType.Blue => blueNote,
            NoteType.Yellow => yellowNote,
            NoteType.Magenta => magentaNote,
            NoteType.Cyan => cyanNote,
            NoteType.White => whiteNote,
            _ => redNote
        };
    }
}
