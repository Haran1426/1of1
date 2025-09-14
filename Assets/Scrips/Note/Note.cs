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
    public float perfectRange = 0.15f;
    public float hitRange = 0.5f;
    public float missThreshold = 1.2f;

    private bool isHit = false;

    [Header("시각 효과")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer visibleNeonSr;
    [SerializeField] private float flashTime = 0.12f;
    [SerializeField] private float flashIntensity = 1.0f;

    [Header("효과음")]
    [SerializeField] private AudioSource audioSource; // Inspector에서 넣을 AudioSource
    [SerializeField] private AudioClip perfectSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missSound;

    void Start()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = GetColorForNoteType(noteType);
    }

    void Update()
    {
        if (isHit) return;

        float distance = Mathf.Abs(transform.position.x - hitZoneX);

        // G 키 입력으로 타격
        if (Input.GetKeyDown(KeyCode.G) && distance <= hitRange)
        {
            isHit = true;

            var type = (distance <= perfectRange)
                ? InGameUIManager.JudgementType.Perfect
                : InGameUIManager.JudgementType.Hit;

            PlayNoteSound(type); // 🎵 사운드
            InGameUIManager.Instance.HandleJudgement(type, transform.position);

            Destroy(gameObject);
            return;
        }

        // Miss 판정
        if (transform.position.x < hitZoneX - missThreshold)
        {
            isHit = true;
            PlayNoteSound(InGameUIManager.JudgementType.Miss); // 🎵 Miss 사운드
            InGameUIManager.Instance.HandleJudgement(InGameUIManager.JudgementType.Miss, transform.position);

            Destroy(gameObject);
        }
    }

    private void PlayNoteSound(InGameUIManager.JudgementType type)
    {
        if (audioSource == null) return;

        switch (type)
        {
            case InGameUIManager.JudgementType.Perfect:
                if (perfectSound != null) audioSource.PlayOneShot(perfectSound);
                break;
            case InGameUIManager.JudgementType.Hit:
                if (hitSound != null) audioSource.PlayOneShot(hitSound);
                break;
            case InGameUIManager.JudgementType.Miss:
                if (missSound != null) audioSource.PlayOneShot(missSound);
                break;
        }
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
}
