using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteSpawner.NoteType noteType;

    [Header("노트 이동 속도")]
    public float moveSpeed = 3f;

    [Header("판정 X 위치")]
    public float hitZoneX = -4.3f;

    [Header("판정 범위 설정")]
    [Tooltip("Perfect 판정 허용 거리")]
    public float perfectRange = 0.15f;
    [Tooltip("Hit 판정 허용 거리")]
    public float hitRange = 0.5f;
    [Tooltip("Miss 판정 시작 거리")]
    public float missThreshold = 1.2f;

    private bool isHit = false;

    [Header("시각 효과")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer visibleNeonSr;

    [Tooltip("판정 플래시 지속시간(노트 본체)")]
    [SerializeField] private float flashTime = 0.12f;
    [Tooltip("플래시 강도 배율(노트 본체)")]
    [SerializeField] private float flashIntensity = 1.0f;

    void Start()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = GetColorForNoteType(noteType);
    }

    void Update()
    {
        if (isHit) return;

        float distance = Mathf.Abs(transform.position.x - hitZoneX);

        // ✅ 테스트: G키로만 판정
        if (Input.GetKeyDown(KeyCode.G) && distance <= hitRange)
        {
            isHit = true;

            var type = (distance <= perfectRange)
                ? InGameUIManager.JudgementType.Perfect
                : InGameUIManager.JudgementType.Hit;

            FlashByJudgement(type);
            PulseVisibleNeon(type);
            InGameUIManager.Instance.HandleJudgement(type, transform.position);

            Destroy(gameObject);
            return;
        }

        // Miss 판정
        if (transform.position.x < hitZoneX - missThreshold)
        {
            isHit = true;
            FlashByJudgement(InGameUIManager.JudgementType.Miss);
            PulseVisibleNeon(InGameUIManager.JudgementType.Miss);
            InGameUIManager.Instance.HandleJudgement(InGameUIManager.JudgementType.Miss, transform.position);
            Destroy(gameObject);
        }
    }

    // --- 기존 Flash, Pulse 코드는 그대로 유지 ---
    private void FlashByJudgement(InGameUIManager.JudgementType jt)
    {
        if (sr == null) return;
        Color flash = GetColorForJudgement(jt);
        StopAllCoroutines();
        StartCoroutine(Co_Flash(flash));
    }

    private IEnumerator Co_Flash(Color flash)
    {
        Color original = GetColorForNoteType(noteType);

        Color boosted = new Color(
            Mathf.Clamp01(flash.r * (1f + flashIntensity)),
            Mathf.Clamp01(flash.g * (1f + flashIntensity)),
            Mathf.Clamp01(flash.b * (1f + flashIntensity)),
            1f
        );

        sr.color = boosted;
        yield return new WaitForSeconds(flashTime);
        sr.color = original;
    }

    private void PulseVisibleNeon(InGameUIManager.JudgementType jt)
    {
        if (visibleNeonSr == null) return;
        StartCoroutine(Co_VisibleNeonPulse(GetColorForJudgementNeon(jt)));
    }

    private IEnumerator Co_VisibleNeonPulse(Color c)
    {
        float timer = 0f;
        float duration = 0.18f;

        visibleNeonSr.color = c * 0.6f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float k = timer / duration;
            float intensity = (k <= 0.5f)
                ? Mathf.Lerp(0.6f, 2.2f, k * 2f)
                : Mathf.Lerp(2.2f, 0.6f, (k - 0.5f) * 2f);

            visibleNeonSr.color = c * intensity;
            yield return null;
        }

        visibleNeonSr.color = Color.white;
    }

    private Color GetColorForJudgement(InGameUIManager.JudgementType jt)
    {
        switch (jt)
        {
            case InGameUIManager.JudgementType.Perfect: return new Color(0.30f, 0.60f, 1.00f);
            case InGameUIManager.JudgementType.Hit: return new Color(1.00f, 0.80f, 0.25f);
            case InGameUIManager.JudgementType.Miss: return new Color(1.00f, 0.30f, 0.35f);
            default: return Color.white;
        }
    }

    private Color GetColorForJudgementNeon(InGameUIManager.JudgementType jt)
    {
        return GetColorForJudgement(jt);
    }

    private Color GetColorForNoteType(NoteSpawner.NoteType t)
    {
        switch (t)
        {
            case NoteSpawner.NoteType.Red: return new Color(1f, 0.2f, 0.2f);
            case NoteSpawner.NoteType.Green: return new Color(0.2f, 1f, 0.2f);
            case NoteSpawner.NoteType.Blue: return new Color(0.2f, 0.6f, 1f);
            case NoteSpawner.NoteType.Yellow: return new Color(1f, 0.85f, 0.2f);
            case NoteSpawner.NoteType.Magenta: return new Color(1f, 0.4f, 1f);
            case NoteSpawner.NoteType.Cyan: return new Color(0.3f, 1f, 1f);
            case NoteSpawner.NoteType.White: return new Color(1f, 1f, 1f);
            default: return Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌 감지: " + other.name + " / Tag: " + other.tag);

        if (other.CompareTag("Skill"))
        {
            InGameUIManager.Instance.OnSkillScoreOnly();
            Destroy(gameObject);
        }
    }

}
