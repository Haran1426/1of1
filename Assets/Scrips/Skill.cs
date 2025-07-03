// Skill.cs
using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("히트박스 프리팹")]
    [SerializeField] private GameObject hitboxPrefab;

    [Header("스킬 설정")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float sweepDuration = 0.5f;
    [SerializeField] private float startX = -4.3f;
    [SerializeField] private float endX = 6.3f;

    private bool isCooldown = false;
    private float cooldownTimer;

    void Update()
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
        isCooldown = true;
        cooldownTimer = cooldown;

        // 히트박스 생성
        Vector2 pos = new Vector2(startX, transform.position.y);
        var hitbox = Instantiate(hitboxPrefab, pos, Quaternion.identity);

        // Sweep 애니메이션
        float t = 0f;
        while (t < sweepDuration)
        {
            float x = Mathf.Lerp(startX, endX, t / sweepDuration);
            hitbox.transform.position = new Vector2(x, transform.position.y);
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(hitbox);
    }

    // 히트박스 충돌만 담당 → 점수는 UI매니저로
    private class HitboxHandler : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Note")) return;

            // 스킬 히트 시: 점수만 추가
            InGameUIManager.Instance.OnSkillScoreOnly();

            // 스포너 관리(있으면)
            var spawner = FindObjectOfType<NoteSpawner>();
            spawner?.RemoveFromList(other.gameObject);

            Destroy(other.gameObject);
        }
    }
}
