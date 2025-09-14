using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 발사체 Prefab (SpriteRenderer만 있는 프리팹)")]
    [SerializeField] private GameObject skillPrefab;

    private const float cooldown = 5f;       // 스킬 쿨타임 (5초)
    private const float sweepDuration = 1f; // 이동 시간
    private const float startX = -4.3f;
    private const float endX = 6.3f;
    private const float detectRadius = 0.5f;  // 노트 충돌 판정 반경

    private bool isCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
            StartCoroutine(CastSkill());
    }

    private IEnumerator CastSkill()
    {
        isCooldown = true;

        // 발사체 생성
        Vector3 spawnPos = new Vector3(startX, transform.position.y, 0f);
        var proj = Instantiate(skillPrefab, spawnPos, Quaternion.identity);

        // 이동 & 충돌 체크
        float elapsed = 0f;
        while (elapsed < sweepDuration)
        {
            float t = elapsed / sweepDuration;
            float x = Mathf.Lerp(startX, endX, t);
            Vector3 pos = new Vector3(x, transform.position.y, 0f);
            proj.transform.position = pos;

            // Note 태그 충돌 체크
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, detectRadius);
            foreach (var c in hits)
            {
                if (c.CompareTag("Note"))
                {
                    int pts = Random.Range(100, 151);
                    InGameUIManager.Instance.OnSkillScoreOnly();

                    FindObjectOfType<NoteSpawner>()?.RemoveFromList(c.gameObject);
                    Destroy(c.gameObject);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(proj);

        // 5초 쿨타임
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
