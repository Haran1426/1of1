using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour

{
    void Start()
    {
        if (timingManager == null)
        {
            Debug.LogError("TimingManager가 연결되지 않았습니다!");
        }
    }

    [Header("리듬 설정")]
    public int bpm = 120;
    private double currentTime = 0d;

    [Header("노트 설정")]
    public GameObject objectPrefab;
    public float spawnX = 6f;
    public float moveSpeed = 7f;
    public int maxObjects = 5;
    
    [Header("참조")]
    public TimingManager timingManager;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    // 노트 생성 위치 (한 개만 랜덤하게 선택될 예정)
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm && spawnedObjects.Count < maxObjects)
        {
            SpawnNote();
            currentTime -= 60d / bpm;
        }

        // 테스트 키 입력
        if (Input.GetKeyDown(KeyCode.Space) && spawnedObjects.Count > 0)
        {
            GameObject note = spawnedObjects[0];
            string result = timingManager.CheckTiming(note);
            Debug.Log("판정 결과: " + result);
            RemoveFromList(note);
            Destroy(note);
        }
    }

    void SpawnNote()
    {
        // 하나의 Y 좌표만 무작위로 선택
        float randomY = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 랜덤 색상
        Color[] colors = { Color.red, Color.green, Color.blue };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr == null) sr = note.AddComponent<SpriteRenderer>();
        sr.color = randomColor;

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

        // 자동 제거 스크립트
        if (note.GetComponent<DestroyOnCutLine>() == null)
        {
            note.AddComponent<DestroyOnCutLine>();
        }

        spawnedObjects.Add(note);
        timingManager.AddNote(note);
    }

    public void RemoveFromList(GameObject note)
    {
        if (spawnedObjects.Contains(note))
        {
            spawnedObjects.Remove(note);
        }

        timingManager.RemoveNote(note);
    }
}
