using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public float spawnX = 4.2f; // 생성될 x 좌표
    public float moveSpeed = 8f; // 이동 속도
    public int maxObjects = 5; // 최대 오브젝트 개수

    private List<GameObject> spawnedObjects = new List<GameObject>(); // 생성된 오브젝트 리스트

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, 0.5f); // 0.5초마다 실행
    }

    void SpawnObject()
    {
        if (spawnedObjects.Count >= maxObjects)
        {
            return; // 최대 개수 초과 시 생성 중지
        }

        if (objectPrefab == null)
        {
            Debug.LogError("🚨 objectPrefab이 설정되지 않았습니다! Unity 인스펙터에서 할당하세요.");
            return;
        }

        float[] yPositions = { 1.5f, 0f, -1.5f };
        float randomY = yPositions[Random.Range(0, yPositions.Length)];

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // 이동 및 충돌 감지를 위한 설정
        Rigidbody2D rb = spawnedObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 영향 X
        rb.velocity = Vector2.left * moveSpeed; // 왼쪽으로 이동

        BoxCollider2D collider = spawnedObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // 충돌 감지 활성화

        spawnedObject.AddComponent<DestroyOnCutLine>(); // Cut Line 충돌 감지 스크립트 추가

        spawnedObjects.Add(spawnedObject); // 리스트에 추가
    }

    // ✅ 리스트에서 제거하는 함수 (Cut Line에 닿으면 자동 호출됨)
    public void RemoveFromList(GameObject obj)
    {
        spawnedObjects.Remove(obj);
    }
}

// ✅ Cut Line 충돌 시 제거하는 스크립트
public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CutLine")) // Cut Line과 충돌하면 삭제
        {
            Point pointScript = FindObjectOfType<Point>();
            if (pointScript != null)
            {
                pointScript.RemoveFromList(gameObject); // 리스트에서 제거
            }

            Destroy(gameObject); // 오브젝트 삭제
        }
    }
}
