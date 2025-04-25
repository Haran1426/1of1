using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("리듬 설정")]
    public int bpm = 120;
    private double currentTime = 0d;

    [Header("노트 설정")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 7f;
    public int maxObjects = 5;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    void Start()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("objectPrefab이 연결되지 않았습니다!");
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm)
        {
            SpawnNote();
            currentTime -= 60d / bpm;
        }
    }

    void SpawnNote()
    {
        float randomY = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 색상 및 NoteType 설정
        Color[] colors = {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            Color.white
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

        int rand = Random.Range(0, colors.Length);
        note.GetComponent<SpriteRenderer>().color = colors[rand];

        NoteColor noteColor = note.AddComponent<NoteColor>();
        noteColor.noteType = types[rand];

        // Rigidbody2D 설정
        Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = note.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
        rb.velocity = Vector2.left * moveSpeed;

        // Collider 설정
        if (note.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = note.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        // 충돌 감지용 스크립트 추가
        if (note.GetComponent<NoteCollision>() == null)
        {
            note.AddComponent<NoteCollision>();
        }

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
