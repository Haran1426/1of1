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
    public float perfectRange = 0.15f;    // 기존 0.1 → 넉넉히
    [Tooltip("Hit 판정 허용 거리")]
    public float hitRange = 0.5f;         // 기존 0.3 → 넉넉히
    [Tooltip("Miss 판정 시작 거리")]
    public float missThreshold = 1.2f;    // 기존 1.0 → 늦게 Miss

    private bool isHit = false;


    [Header("시각 효과")]
    [SerializeField] private SpriteRenderer sr;            // 노트 자체 스프라이트(없어도 동작)
    [SerializeField] private SpriteRenderer visibleNeonSr; // 판정선/보여지는 네온 SpriteRenderer(필수)

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

        // Perfect / Hit 판정
        if (Input.anyKeyDown && distance <= hitRange && CheckInput(noteType))
        {
            isHit = true;

            // 어떤 판정인지 결정
            var type = (distance <= perfectRange)
                ? InGameUIManager.JudgementType.Perfect
                : InGameUIManager.JudgementType.Hit;

            FlashByJudgement(type);           // 노트 본체 짧은 플래시(선택사항)
            PulseVisibleNeon(type);           // 판정선(보이는 오브젝트) 네온 펄스


            // UI 이미지/이펙트/점수 처리
            InGameUIManager.Instance.HandleJudgement(type, transform.position);

            Destroy(gameObject);
            return;
        }

        //판정
        if (transform.position.x < hitZoneX - missThreshold)
        {
            isHit = true;
            FlashByJudgement(InGameUIManager.JudgementType.Miss);
            PulseVisibleNeon(InGameUIManager.JudgementType.Miss);
  

            InGameUIManager.Instance.HandleJudgement(InGameUIManager.JudgementType.Miss, transform.position);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 단일 또는 조합 키 입력을 true/false로 반환
    /// </summary>
    private bool CheckInput(NoteSpawner.NoteType type)
    {
        bool r = Input.GetKeyDown(KeyCode.R);
        bool g = Input.GetKeyDown(KeyCode.G);
        bool b = Input.GetKeyDown(KeyCode.B);

        switch (type)
        {
            case NoteSpawner.NoteType.Red: return r;
            case NoteSpawner.NoteType.Green: return g;
            case NoteSpawner.NoteType.Blue: return b;
            case NoteSpawner.NoteType.Yellow: return (Input.GetKey(KeyCode.R) && g) || (Input.GetKey(KeyCode.G) && r);
            case NoteSpawner.NoteType.Magenta: return (Input.GetKey(KeyCode.R) && b) || (Input.GetKey(KeyCode.B) && r);
            case NoteSpawner.NoteType.Cyan: return (Input.GetKey(KeyCode.G) && b) || (Input.GetKey(KeyCode.B) && g);
            case NoteSpawner.NoteType.White:
                return
                    (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.G) && b) ||
                    (Input.GetKey(KeyCode.R) && g && Input.GetKey(KeyCode.B)) ||
                    (r && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.B));
            default: return false;
        }
    }

    // 노트 본체: 판정색으로 짧게 번쩍(없어도 됨, sr 미할당 시 패스)
    private void FlashByJudgement(InGameUIManager.JudgementType jt)
    {
        if (sr == null) return;
        Color flash = GetColorForJudgement(jt); //판정 기준 컬러
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

    // [CHANGE-5] "보여지는 오브젝트(네온)"에만 판정색 펄스
    private void PulseVisibleNeon(InGameUIManager.JudgementType jt)
    {
        if (visibleNeonSr == null) return;
        StartCoroutine(Co_VisibleNeonPulse(GetColorForJudgementNeon(jt)));
    }

    private IEnumerator Co_VisibleNeonPulse(Color c)
    {
        float timer = 0f;
        float duration = 0.18f;

        // 시작은 약간 어둡게
        visibleNeonSr.color = c * 0.6f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float k = timer / duration;
            // 0→피크→0 형태의 펄스(간단한 삼각 보간)
            float intensity = (k <= 0.5f)
                ? Mathf.Lerp(0.6f, 2.2f, k * 2f)
                : Mathf.Lerp(2.2f, 0.6f, (k - 0.5f) * 2f);

            visibleNeonSr.color = c * intensity;
            yield return null;
        }

        // 네온 기본색으로 복귀(흰색 또는 평상시 네온색이 있다면 거기에 맞춰 바꿔도 됨)
        visibleNeonSr.color = Color.white;
    }
    private Color GetColorForJudgement(InGameUIManager.JudgementType jt)
    {
        switch (jt)
        {
            case InGameUIManager.JudgementType.Perfect: return new Color(0.30f, 0.60f, 1.00f); // 파랑
            case InGameUIManager.JudgementType.Hit: return new Color(1.00f, 0.80f, 0.25f); // 노랑~주황 느낌
            case InGameUIManager.JudgementType.Miss: return new Color(1.00f, 0.30f, 0.35f); // 빨강
            default: return Color.white;
        }
    }

    // 네온 전용(같은 값 사용하지만 분리해두면 나중에 독립 조정 용이)
    private Color GetColorForJudgementNeon(InGameUIManager.JudgementType jt)
    {
        return GetColorForJudgement(jt);
    }

    // 타입 기본색(노트 본체 기본색으로만 사용)
    private Color GetColorForNoteType(NoteSpawner.NoteType t)
    {
        switch (t)
        {
            case NoteSpawner.NoteType.Red: return new Color(1f, 0.2f, 0.2f);
            case NoteSpawner.NoteType.Green: return new Color(0.2f, 1f, 0.2f);
            case NoteSpawner.NoteType.Blue: return new Color(0.2f, 0.6f, 1f);
            case NoteSpawner.NoteType.Yellow: return new Color(1f, 0.85f, 0.2f); // R+G
            case NoteSpawner.NoteType.Magenta: return new Color(1f, 0.4f, 1f);    // R+B
            case NoteSpawner.NoteType.Cyan: return new Color(0.3f, 1f, 1f);    // G+B
            case NoteSpawner.NoteType.White: return new Color(1f, 1f, 1f);      // R+G+B
            default: return Color.white;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("CutLine"))
        {

            Destroy(gameObject);
        }
    }
}
