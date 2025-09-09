using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Skill : MonoBehaviour
{
    [Header("스킬 오브젝트 Prefab (인스펙터에서 드래그)")]
    [SerializeField] private GameObject skillPrefab;

    // → Inspector 조정 없이 코드에 하드코딩
    private const float cooldown = 5f;    // 5초마다 사용
    private const float sweepDuration = 0.5f;  // (–4.3→6.3) 이동에 걸리는 시간
    private const float startX = -4.3f;
    private const float endX = 6.3f;
    private const float detectRadius = 0.5f;  // 노트 충돌 감지 반경

    private bool isCooldown = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
            StartCoroutine(CastSkill());
    }

    private IEnumerator CastSkill()
    {
        isCooldown = true;

        // 1) 스킬 오브젝트 생성
        Vector3 spawnPos = new Vector3(startX, transform.position.y, 0f);
        var proj = Instantiate(skillPrefab, spawnPos, Quaternion.identity);

        // 2) –4.3 → 6.3 스윕 & 노트 감지
        float elapsed = 0f;
        while (elapsed < sweepDuration)
        {
            float t = elapsed / sweepDuration;
            float x = Mathf.Lerp(startX, endX, t);
            Vector3 pos = new Vector3(x, transform.position.y, 0f);
            proj.transform.position = pos;

            // OverlapCircleAll로 주변 Note 태그 충돌 체크
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos, detectRadius);
            foreach (var c in hits)
            {
                if (c.CompareTag("Note"))
                {
                    // 점수 처리 (100~150)
                    int pts = Random.Range(100, 151);
                    InGameUIManager.Instance.OnSkillScoreOnly();

                    // 스포너 리스트에서 제거
                    FindObjectOfType<NoteSpawner>()?.RemoveFromList(c.gameObject);

                    Destroy(c.gameObject);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(proj);

        // 3) 쿨타임 대기
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }

    // (선택) Scene 뷰에 감지 반경 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(
            new Vector3(startX, transform.position.y, 0f),
            detectRadius
        );
        Gizmos.DrawWireSphere(
            new Vector3(endX, transform.position.y, 0f),
            detectRadius
        );
    }
}
