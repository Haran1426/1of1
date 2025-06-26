using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    [Header("히트박스 프리팹 (인스펙터 할당)")]
    [SerializeField] private GameObject hitboxPrefab;

    [Header("스킬 설정")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float sweepDuration = 0.5f;
    [SerializeField] private float startX = -4.3f;
    [SerializeField] private float endX = 6.3f;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
            StartCoroutine(ActivateSkill());

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                isCooldown = false;
        }
    }

    private IEnumerator ActivateSkill()
    {
        // 1) 쿨다운 시작
        isCooldown = true;
        cooldownTimer = cooldown;

        // 2) 히트박스 생성
        Vector2 spawnPos = new Vector2(startX, transform.position.y);
        GameObject hitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);

        // → [삭제된 부분]
        //    Rigidbody2D, Collider2D 자동 추가 코드 제거
        //    if (!hitbox.TryGetComponent<Rigidbody2D>(out var rb)) { … }
        //    if (!hitbox.TryGetComponent<Collider2D>(out var col)) { … }

        // 3) 충돌 처리 스크립트는 아직 없다면 추가
        if (!hitbox.TryGetComponent<HitboxHandler>(out _))
            hitbox.AddComponent<HitboxHandler>();

        // 4) 스윕 이동
        float elapsed = 0f;
        while (elapsed < sweepDuration)
        {
            float t = elapsed / sweepDuration;
            float x = Mathf.Lerp(startX, endX, t);
            hitbox.transform.position = new Vector2(x, transform.position.y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(hitbox);
    }

    // 내부 클래스: 노트 충돌 처리
    private class HitboxHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Note"))
            {
                int points = Random.Range(100, 151);
                Score.Instance?.AddScore(points);

                var spawner = FindObjectOfType<NoteSpawner>();
                if (spawner != null)
                    spawner.RemoveFromList(other.gameObject);

                Destroy(other.gameObject);
            }
        }
    }
}
