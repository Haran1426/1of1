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
    public float spawnX = 4.2f;
    public float moveSpeed = 10f;
    public int maxObjects = 5;

    [Header("참조")]
    public TimingManager timingManager; // 💡 타이밍 매니저 연결

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Update()
    {
        currentTime += Time.deltaTime;

        // 노트가 없을 때만 생성 (1개만 나오는 조건)
        if (currentTime >= 60d / bpm && spawnedObjects.Count < maxObjects)
        {
            SpawnNote(); // 🎯 단 한 번만 실행
            currentTime -= 60d / bpm;
        }

        // 테스트용 키 입력 시 가장 앞에 있는 노트의 판정 체크
        if (Input.GetKeyDown(KeyCode.Space) && spawnedObjects.Count > 0)
        {
            GameObject note = spawnedObjects[0];
            string result = timingManager.CheckTiming(note);
            Debug.Log("판정 결과: " + result);

            // 판정 후 노트 제거 (예시)
            RemoveFromList(note);
            Destroy(note);
        }
    }

    void SpawnNote()
    {
        // SpawnNote 함수 안에서 라인 Y좌표를 랜덤하게 선택
        float[] yPositions = { 1.5f, 0f, -1.5f };
        float randomY = yPositions[Random.Range(0, yPositions.Length)];

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);


        // 색상 랜덤 지정
        Color[] colors = { Color.red, Color.green, Color.blue };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
        if (sr == null) sr = note.AddComponent<SpriteRenderer>();
        sr.color = randomColor;

        // 물리 설정
        if (note.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = note.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = Vector2.left * moveSpeed;
        }

        // 충돌 설정
        if (note.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = note.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        // 노트 자동 삭제 스크립트
        if (note.GetComponent<DestroyOnCutLine>() == null)
        {
            note.AddComponent<DestroyOnCutLine>();
        }

        spawnedObjects.Add(note);

        // 💡 TimingManager에도 노트 등록
        timingManager.AddNote(note);
    }

    public void RemoveFromList(GameObject note)
    {
        if (spawnedObjects.Contains(note))
        {
            spawnedObjects.Remove(note);
        }

        timingManager.RemoveNote(note); // 💡 TimingManager에서도 제거
    }
}
