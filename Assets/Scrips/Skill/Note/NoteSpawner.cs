using UnityEngine;
using System.Collections.Generic;
using static MapCreator; // MapData, NoteData 구조 사용

public class NoteSpawner : MonoBehaviour
{
    [Header("맵 및 사운드")]
    public AudioSource musicSource;
    public string mapName = "Go_On"; // Resources/Maps/Go_On.json

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

    public enum NoteType { Red, Green, Blue, Yellow, Magenta, Cyan, White }

    // 스킬 등 외부에서 제거할 때 참조할 리스트
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    private MapData mapData;
    private int currentNoteIndex = 0;

    void Start()
    {
        // 맵 데이터 로드
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
        float timeToReach = mapData.notes[currentNoteIndex].time - GetNoteDelay();

        if (timeToReach <= currentTime)
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
    void SpawnNote(MapCreator.NoteData noteData)
    {
        if (spawnedObjects.Count >= maxObjects) return;

        float y = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPos = new Vector3(spawnX, y, 0f);
        GameObject note = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        NoteType type = ConvertType(noteData.type); // 문자열 기반

        // 스프라이트 설정
        var sr = note.GetComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForType(type);

        // Note 스크립트 부착
        var noteScript = note.AddComponent<Note>();
        noteScript.noteType = type;
        noteScript.moveSpeed = moveSpeed;
        noteScript.hitZoneX = HitLine.position.x;

        // 물리 이동
        var rb = note.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * moveSpeed;

        note.tag = "Note";
        spawnedObjects.Add(note);
    }
    NoteType ConvertType(string type)
    {
        return type switch
        {
            "R" => NoteType.Red,
            "G" => NoteType.Green,
            "B" => NoteType.Blue,
            "Y" => NoteType.Yellow,
            "M" => NoteType.Magenta,
            "C" => NoteType.Cyan,
            "W" => NoteType.White,
            _ => NoteType.Red,
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
            _ => redNote,
        };
    }

    // Skill 등 외부에서 호출: 내부 리스트에서 제거
    public void RemoveFromList(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
            spawnedObjects.Remove(obj);
    }
}
