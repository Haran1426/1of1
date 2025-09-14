using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 발사체 Prefab (Collider2D + Rigidbody2D 붙은 프리팹)")]
    [SerializeField] private GameObject skillPrefab;

    private const float cooldown = 30f;     // 쿨타임
    private const float sweepDuration = 1f; // 이동 시간
    private const float startX = -4.3f;
    private const float endX = 6.3f;

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

        // 발사체 이동 (좌→우)
        float elapsed = 0f;
        while (elapsed < sweepDuration)
        {
            float t = elapsed / sweepDuration;
            float x = Mathf.Lerp(startX, endX, t);
            proj.transform.position = new Vector3(x, transform.position.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(proj);

        // 쿨타임 대기
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
