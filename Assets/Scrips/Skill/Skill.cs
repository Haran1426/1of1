using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("스킬 발사체 Prefab (BoxCollider2D + IsTrigger 필요)")]
    [SerializeField] private GameObject skillPrefab;

    private const float cooldown = 30f;
    private const float sweepDuration = 1f;
    private const float startX = -4.3f;
    private const float endX = 6.3f;

    private bool isCooldown = false;

    [Header("사운드")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip skillCastSound; // 발사음

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooldown)
            StartCoroutine(CastSkill());
    }

    private IEnumerator CastSkill()
    {
        isCooldown = true;

        // 발사 사운드
        if (audioSource != null && skillCastSound != null)
            audioSource.PlayOneShot(skillCastSound);

        // 발사체 생성
        Vector3 spawnPos = new Vector3(startX, transform.position.y, 0f);
        var proj = Instantiate(skillPrefab, spawnPos, Quaternion.identity);

        // 이동
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
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
