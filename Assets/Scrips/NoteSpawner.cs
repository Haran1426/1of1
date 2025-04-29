using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("리듬 설정")]
    public AudioSource musicSource;
    public int bpm = 120;
    private float spawnInterval;
    private float nextSpawnTime;

    [Header("노트 설정")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 7f;
    public int maxObjects = 999;

    [Header("노트 스프라이트")]
    public Sprite redNote;
    public Sprite greenNote;
    public Sprite blueNote;
    public Sprite yellowNote;
    public Sprite magentaNote;
    public Sprite cyanNote;
    public Sprite whiteNote;

    private List<GameObject> spawnedObjects = new();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    void Start()
    {
        if (objectPrefab == null || musicSource == null)
        {
            Debug.LogError("NoteSpawner: 오브젝트 프리팹 또는 음악이 연결되지 않았습니다!");
            return;
        }

        spawnInterval = 60f / bpm;
        nextSpawnTime = 0f;
        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        while (musicSource.time >= nextSpawnTime)
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

        // 랜덤 스프라이트 지정
        Sprite[] sprites = {
            redNote, greenNote, blueNote, yellowNote, magentaNote, cyanNote, whiteNote
        };

        NoteColor.NoteType[] types = {
            NoteColor.NoteType.Red,
            NoteColor.NoteType.Green,
            NoteColor.NoteType.Blue,
            NoteColor.NoteType.Yellow,
            NoteColor.NoteType.Magenta,
            NoteColor.NoteType.Cyan,
            NoteColor.NoteType.White
        };

        int rand = Random.Range(0, sprites.Length);
        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr == null) sr = note.AddComponent<SpriteRenderer>();
        sr.sprite = sprites[rand];

        NoteColor nc = note.AddComponent<NoteColor>();
        nc.noteType = types[rand];

        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = note.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
        rb.velocity = Vector2.left * moveSpeed;

        BoxCollider2D col = note.GetComponent<BoxCollider2D>();
        if (col == null) col = note.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        if (note.GetComponent<NoteCollision>() == null)
            note.AddComponent<NoteCollision>();

        spawnedObjects.Add(note);
    }

    public void RemoveFromList(GameObject note)
    {
        if (spawnedObjects.Contains(note))
        {
            spawnedObjects.Remove(note);
        }
    }
}
