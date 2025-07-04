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
    public float hitRange = 0.5f;     // 기존 0.3 → 넉넉히
    [Tooltip("Miss 판정 시작 거리")]
    public float missThreshold = 1.2f;    // 기존 1.0 → 늦게 Miss

    private bool isHit = false;

    void Update()
    {
        if (isHit) return;

        float distance = Mathf.Abs(transform.position.x - hitZoneX);

        // 1) Perfect / Hit 판정
        if (Input.anyKeyDown && distance <= hitRange && CheckInput(noteType))
        {
            isHit = true;

            // 어떤 판정인지 결정
            var type = (distance <= perfectRange)
                ? InGameUIManager.JudgementType.Perfect
                : InGameUIManager.JudgementType.Hit;

            // UI 이미지/이펙트/점수 처리
            InGameUIManager.Instance.HandleJudgement(type, transform.position);

            Destroy(gameObject);
            return;
        }

        // 2) Miss 판정: 판정선에서 너무 벗어났을 때
        if (transform.position.x < hitZoneX - missThreshold)
        {
            isHit = true;
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
}
