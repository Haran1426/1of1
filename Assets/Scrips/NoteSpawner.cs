using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("리듬 설정")]
    public AudioSource musicSource; // 🎵 노래 연결
    public int bpm = 120;
    private float spawnInterval;
    private float nextSpawnTime;

    [Header("노트 설정")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 7f;
    public int maxObjects = 999;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    [Header("노트 사전 이동 보정")]
    public float judgeX = -4.5f;  // 🎯 판정 위치 (노트가 도착해야 할 X 좌표)
    private float noteTravelTime; // 노트가 이동하는 데 걸리는 시간

    void Start()
    {
        if (objectPrefab == null || musicSource == null)
        {
            Debug.LogError("NoteSpawner: 오브젝트 프리팹이나 음악이 연결되지 않았습니다!");
            return;
        }

        spawnInterval = 60f / bpm; // 1비트마다 스폰
        nextSpawnTime = 0f;

        noteTravelTime = Mathf.Abs(spawnX - judgeX) / moveSpeed; // 📏 이동 시간 계산

        musicSource.Play(); // 🎵 음악 시작
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        // 음악 시간 + 이동 시간으로 스폰 타이밍을 미리 땡긴다
        while (musicSource.time + noteTravelTime >= nextSpawnTime)
        {
            SpawnNote();
            nextSpawnTime += spawnInterval;
        }
    }

    void SpawnNote()
    {
        if (spawnedObjects.Count >= maxObjects) return;

        float randomY = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 랜덤 색상 및 타입 설정
        Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, Color.white };
        NoteColor.NoteType[] types = {
            NoteColor.NoteType.Red,
            NoteColor.NoteType.Green,
            NoteColor.NoteType.Blue,
            NoteColor.NoteType.Yellow,
            NoteColor.NoteType.Magenta,
            NoteColor.NoteType.Cyan,
            NoteColor.NoteType.White
        };

        int rand = Random.Range(0, colors.Length);
        var sr = note.GetComponent<SpriteRenderer>();
        if (sr == null) sr = note.AddComponent<SpriteRenderer>();
        sr.color = colors[rand];

        var noteColor = note.AddComponent<NoteColor>();
        noteColor.noteType = types[rand];

        var rb = note.GetComponent<Rigidbody2D>();
        if (rb == null) rb = note.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = Vector2.left * moveSpeed;

        var col = note.GetComponent<BoxCollider2D>();
        if (col == null) col = note.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        if (note.GetComponent<NoteCollision>() == null)
            note.AddComponent<NoteCollision>();

        spawnedObjects.Add(note);
    }
}
