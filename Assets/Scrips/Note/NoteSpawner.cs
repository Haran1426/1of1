using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    [Header("맵 및 사운드")]
    public AudioSource musicSource;
    public string mapName = "2000_balanced_v9"; // Resources/Maps/MapData.json

    [Header("노트 설정")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 3f;
    public int maxObjects = 999;

    [Header("노트 스프라이트")]
    public Sprite redNote, greenNote, blueNote;
    public Sprite yellowNote, magentaNote, cyanNote, whiteNote;

    [Header("판정선 오브젝트")]
    public Transform HitLine;

    public enum NoteType { Red = 1, Green, Blue, Yellow, Magenta, Cyan, White }

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private MapData mapData;
    private int currentNoteIndex = 0;

    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Maps/{mapName}");
        if (jsonFile == null)
        {
            Debug.LogError("맵 파일을 찾을 수 없습니다: " + mapName);
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
        if (currentNoteIndex >= mapData.notes.Count) return;

        float currentTime = musicSource.time;
        float beatLength = 60f / mapData.bpm;
        float noteTime = mapData.notes[currentNoteIndex].beat * beatLength;

        float spawnDelay = GetNoteDelay();

        if (noteTime - spawnDelay <= currentTime)
        {
            SpawnNote(mapData.notes[currentNoteIndex]);
            currentNoteIndex++;
        }
    }

    float GetNoteDelay()
    {
        float distance = spawnX - HitLine.position.x;
        return distance / moveSpeed;
    }

    void SpawnNote(NoteData noteData)
    {
        if (spawnedObjects.Count >= maxObjects) return;

        Vector3 spawnPos = new Vector3(spawnX, noteData.posY, 0f);
        GameObject note = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        NoteType type = (NoteType)noteData.type;

        var sr = note.GetComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForType(type);

        var noteScript = note.AddComponent<Note>();
        noteScript.noteType = type;
        noteScript.moveSpeed = moveSpeed;
        noteScript.hitZoneX = HitLine.position.x;

        var rb = note.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * moveSpeed;

        note.tag = "Note";
        spawnedObjects.Add(note);
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
            _ => redNote,
        };
    }

    public void RemoveFromList(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
            spawnedObjects.Remove(obj);
    }
}
