using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skill : MonoBehaviour
{
    public Skill SweepSkill;
    public GameObject hitboxPrefab;
    public float cooldown = 5f;
    public float sweepDuration = 0.5f;
    public Vector2 startPos = new Vector2(-4.3f, 0f);
    public Vector2 endPos = new Vector2(6.3f, 0f);

    [Header("쿨타임 UI")]
    public Image cooldownImage;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SweepSkill.TryActivateSkill();
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);

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

        if (!hitbox.TryGetComponent<Rigidbody2D>(out _))
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

    // 충돌 처리 클래스
    private class SkillHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Note"))
            {
                Score.Instance?.AddScore(Random.Range(100, 151));

                NoteSpawner spawner = FindObjectOfType<NoteSpawner>();
                if (spawner != null)
                {
                    spawner.RemoveFromList(other.gameObject);
                }

                Destroy(other.gameObject);
            }
        }
    }
}
