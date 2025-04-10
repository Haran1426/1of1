using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public float spawnX = 4.2f; // 생성될 x 좌표
    public float moveSpeed = 10f; // 이동 속도
    public int maxObjects = 5; // 최대 오브젝트 개수

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트 리스트

    void Start()
    {

        StartCoroutine(SpawnObjects()); // 코루틴 시작
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (spawnedObjects.Count < maxObjects)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(0.5f); // 1초 대기 후 다시 실행
        }
    }

    void SpawnObject()
    {
        if (spawnedObjects.Count >= maxObjects)
        {
            Debug.Log("최대 개수 도달! 생성 중지");
            return;
        }

        float[] yPositions = { 1.5f, 0f, -1.5f };
        float randomY = yPositions[Random.Range(0, yPositions.Length)];

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 🔴🟢🔵 랜덤 색상 설정
        Color[] colors = { Color.red, Color.green, Color.blue };
        Color randomColor = colors[Random.Range(0, colors.Length)];

        SpriteRenderer sr = spawnedObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = spawnedObject.AddComponent<SpriteRenderer>();
        }
        sr.color = randomColor;

        // Rigidbody2D 추가
        if (spawnedObject.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = spawnedObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = Vector2.left * moveSpeed;
        }

        // Collider 추가
        if (spawnedObject.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D collider = spawnedObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        // DestroyOnCutLine 스크립트 추가
        if (spawnedObject.GetComponent<DestroyOnCutLine>() == null)
        {
            spawnedObject.AddComponent<DestroyOnCutLine>();
        }

        spawnedObjects.Add(spawnedObject);
        Debug.Log($"✅ 오브젝트 생성됨! 현재 개수: {spawnedObjects.Count}");
    }


    public void RemoveFromList(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
        {
            spawnedObjects.Remove(obj);
            Debug.Log($"✅ {obj.name} 삭제됨! 현재 개수: {spawnedObjects.Count}");
        }
    }
}