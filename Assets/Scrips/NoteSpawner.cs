using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour

{
    void Awake()
    {
        // 이 컴포넌트가 "NoteSpawner" 태그가 붙은 오브젝트에만 살아 있도록
        if (!CompareTag("NoteSpawner"))
            enabled = false;
    }

    void Start()
    {
        if (timingManager == null)
            timingManager = FindObjectOfType<TimingManager>();

        if (timingManager == null)
            Debug.LogError("TimingManager가 연결되지 않았습니다!");
    }


    [Header("리듬 설정")]
    public int bpm = 100;
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
        // (1) 파괴된 노트(== null) 자동 제거
        spawnedObjects.RemoveAll(note => note == null);

        // (2) 시간 누적
        currentTime += Time.deltaTime;

        // (3) 노트가 없을 때만 BPM 주기에 맞춰 스폰
        if (currentTime >= 60d / bpm && spawnedObjects.Count == 0)
        {
            Debug.Log($"노트 생성! bpm={bpm}, 지난시간={currentTime:F2}");
            SpawnNote();
            currentTime -= 60d / bpm;
        }

        // (4) 키 입력 테스트 (Space 누르면 즉시 제거)
        if (Input.GetKeyDown(KeyCode.Space) && spawnedObjects.Count > 0)
        {
            var note = spawnedObjects[0];
            Debug.Log("RemoveFromList 호출 전 Count=" + spawnedObjects.Count);
            string result = timingManager.CheckTiming(note);
            Debug.Log("판정 결과: " + result);
            RemoveFromList(note);
            Destroy(note);
            Debug.Log("RemoveFromList 호출 후 Count=" + spawnedObjects.Count);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CutLine"))
        {
            FindObjectOfType<NoteSpawner>().RemoveFromList(gameObject);
            Destroy(gameObject);
            Debug.Log("DestroyOnCutLine 에서 RemoveFromList & Destroy 호출");
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
