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



    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60d / bpm && spawnedObjects.Count < maxObjects)
        {
            SpawnNote();
            currentTime -= 60d / bpm;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌한 오브젝트: " + other.gameObject.name);

        if (other.CompareTag("CutLine"))
        {
            Debug.Log("CutLine과 충돌함, 노트 삭제!");
            Destroy(gameObject);
        }
    }

    void SpawnNote()
    {
        float randomY = yPositions[Random.Range(0, yPositions.Length)];
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject note = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        Color[] colors = {
    Color.red,       // R
    Color.green,     // G
    Color.blue,      // B
    Color.yellow,    // R + G
    Color.magenta,   // R + B
    Color.cyan,      // G + B
    Color.white      // R + G + B
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
        Color randomColor = colors[rand];
        note.GetComponent<SpriteRenderer>().color = randomColor;

        // NoteColor 스크립트에 색상 정보 전달
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

        // 데미지 처리 스크립트 추가
        if (note.GetComponent<Damages>() == null)
        {
            note.AddComponent<Damages>();
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
