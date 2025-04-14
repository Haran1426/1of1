using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("리듬 설정")]
    public int bpm = 120; // BPM
    private double currentTime = 0d;

    [Header("노트 설정")]
    public GameObject objectPrefab; // 생성할 노트 프리팹
    public float spawnX = 4.2f; // 생성될 X 좌표
    public float moveSpeed = 10f; // 노트 이동 속도
    public int maxObjects = 5; // 화면에 존재할 최대 노트 개수

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 노트 리스트

    void Update()
    {
        currentTime += Time.deltaTime;

        // 일정 BPM 타이밍에 도달했고, 최대 개수 미만일 때만 노트 생성
        if (currentTime >= 60d / bpm && spawnedObjects.Count < maxObjects)
        {
            SpawnNote();
            currentTime -= 60d / bpm;
        }
    }

    void SpawnNote()
    {
        float[] yPositions = { 1.5f, 0f, -1.5f }; // 노트 Y 위치
        float randomY = yPositions[Random.Range(0, yPositions.Length)];

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 노트 색상 랜덤 설정
        Color[] colors = { Color.red, Color.green, Color.blue };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = note.AddComponent<SpriteRenderer>();
        }
        sr.color = randomColor;

        // Rigidbody2D 설정
        if (note.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = note.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = Vector2.left * moveSpeed;
        }

        // Collider 설정
        if (note.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = note.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        // DestroyOnCutLine 스크립트 추가
        if (note.GetComponent<DestroyOnCutLine>() == null)
        {
            note.AddComponent<DestroyOnCutLine>();
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
