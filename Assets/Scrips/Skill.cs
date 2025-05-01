using UnityEngine;
using UnityEngine.UI; // UI 사용
using System.Collections;

public class Skill : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public float cooldown = 30f;
    public float sweepDuration = 0.5f;
    public Vector2 startPos = new Vector2(-4.3f, 0f);
    public Vector2 endPos = new Vector2(6.3f, 0f);

    [Header("쿨타임 UI")]
    public Image cooldownImage; // Inspector에서 연결

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0f)
                cooldownTimer = 0f;

            if (cooldownImage != null)
                cooldownImage.fillAmount = 1f - (cooldownTimer / cooldown);
        }
    }

    public void TryActivateSkill()
    {
        if (!isCooldown)
            StartCoroutine(ActivateSkill());
    }

    private IEnumerator ActivateSkill()
    {
        isCooldown = true;
        cooldownTimer = cooldown;

        GameObject hitbox = Instantiate(hitboxPrefab);
        hitbox.tag = "Skill";
        hitbox.transform.position = new Vector2(startPos.x, transform.position.y);

        if (hitbox.GetComponent<BoxCollider2D>() == null)
        {
            BoxCollider2D col = hitbox.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }

        if (hitbox.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = hitbox.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        hitbox.AddComponent<SkillHitbox>();

        float elapsed = 0f;
        while (elapsed < sweepDuration)
        {
            float t = elapsed / sweepDuration;
            float newX = Mathf.Lerp(startPos.x, endPos.x, t);
            hitbox.transform.position = new Vector2(newX, transform.position.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(hitbox);

        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    // 충돌 처리
    private class SkillHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Note"))
            {
                Score.Instance?.AddScore(Random.Range(100, 151));

                NoteSpawner spawner = FindObjectOfType<NoteSpawner>();
                if (spawner != null)
                    spawner.RemoveFromList(other.gameObject);

                Destroy(other.gameObject);
            }
        }
    }
}
