using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour
{
    [Header("Rhythm setting")]
    public AudioSource musicSource;
    public int bpm = 120;
    private float spawnInterval;
    private float nextSpawnTime;

    [Header("Note Setting")]
    public GameObject objectPrefab;
    private float spawnX = 6f;
    private float moveSpeed = 2f;
    public int maxObjects = 999;

    [Header("Note Color")]
    public Sprite redNote;
    public Sprite greenNote;
    public Sprite blueNote;
    public Sprite yellowNote;
    public Sprite magentaNote;
    public Sprite cyanNote;
    public Sprite whiteNote;

    [Header("HitLine")]
    public Transform HitLine; // 인스펙터에서 HitLine 드래그해 넣기
    public float judgementRange = 10f; // 판정 범위 설정

    public enum NoteType { Red, Green, Blue, Yellow, Magenta, Cyan, White }
    public NoteType[] types;

    private List<GameObject> spawnedObjects = new();
    private float[] yPositions = { 1.5f, 0f, -1.5f };

    void Start()
    {
        if (objectPrefab == null || musicSource == null) return;

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

        HandleInput(); // ⬅️ 입력 처리
        //HandleMiss();  // ⬅️ 누락된 감지 로직 호출 (추가!)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CutLine"))
        {
            Score.Instance.OnHitCutLine();


        }
    }
    void SpawnNote()
    {
        if (Time.timeScale > 0)
        {
            if (spawnedObjects.Count >= maxObjects) return;

            float randomY = yPositions[Random.Range(0, yPositions.Length)];
            Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
            GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

            // 랜덤 스프라이트 지정
            Sprite[] sprites = { redNote, greenNote, blueNote, yellowNote, magentaNote, cyanNote, whiteNote };
            int rand = Random.Range(0, sprites.Length);
            SpriteRenderer sr = note.GetComponent<SpriteRenderer>();
            sr.sprite = sprites[rand];

            NoteData data = note.AddComponent<NoteData>(); // ⬅️ NoteType 저장
            data.noteType = types[rand];

            Rigidbody2D rb = note.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * moveSpeed;

            note.tag = "Note";

            spawnedObjects.Add(note);
        }
    }

    void HandleInput()
    {
        spawnedObjects.RemoveAll(note => note == null); // 삭제된 노트 제거

        if (Input.anyKeyDown)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                GameObject note = spawnedObjects[i];
                if (note == null) continue;

                float distance = Mathf.Abs(note.transform.position.x - HitLine.position.x);
                if (distance > judgementRange) continue;


                NoteData data = note.GetComponent<NoteData>();
                if (data == null) continue;

                bool correct = false;

                switch (data.noteType)
                {
                    case NoteType.Red:
                        correct = Input.GetKeyDown(KeyCode.R);
                        break;
                    case NoteType.Green:
                        correct = Input.GetKeyDown(KeyCode.G);
                        break;
                    case NoteType.Blue:
                        correct = Input.GetKeyDown(KeyCode.B);
                        break;
                    case NoteType.Yellow:
                        correct = Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.G);
                        break;
                    case NoteType.Magenta:
                        correct = Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.B);
                        break;
                    case NoteType.Cyan:
                        correct = Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.B);
                        break;
                    case NoteType.White:
                        correct = Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.B);
                        break;
                }

                if (correct)
                {
                    Debug.Log("노트 맞음! 점수 증가");
                    if (Score.Instance != null)
                        Score.Instance.OnHitHitBox();
                    else
                        Debug.LogWarning("Score.Instance가 null입니다.");

                    Destroy(note);
                    spawnedObjects.RemoveAt(i);
                    break;
                }

            }
        }
    }

    //void HandleMiss()
    //{
    //    for (int i = spawnedObjects.Count - 1; i >= 0; i--)
    //    {
    //        GameObject note = spawnedObjects[i];
    //        if (note == null) continue;

    //        // 일정 위치 이하로 내려간 노트 감지
    //        if (note.transform.position.x <= -6f)
    //        {
    //            if (Score.Instance != null)
    //            {
    //                Score.Instance.OnHitCutLine();
    //            }



    //            Destroy(note);
    //            spawnedObjects.RemoveAt(i);
    //        }
    //    }
    //}


    public void RemoveFromList(GameObject note)
    {
        if (spawnedObjects.Contains(note))
        {
            spawnedObjects.Remove(note);
        }
    }
}

// ⬇️ NoteData 클래스 (같은 파일에 둬도 무방함)

