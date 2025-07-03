// Note.cs
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteSpawner.NoteType noteType;

    [Header("이동 & 판정 존 설정")]
    [Tooltip("노트 이동 속도")]
    public float moveSpeed = 3f;
    [Tooltip("판정할 X 좌표 (히트 존)")]
    public float hitZoneX = -4.3f;
    [Tooltip("일반 히트 허용 범위")]
    public float hitRange = 0.3f;
    [Tooltip("퍼펙트 히트 허용 범위")]
    public float perfectRange = 0.1f;

    // 외부 입력판정에서 이미 처리된 노트인지 플래그
    [HideInInspector] public bool isHit = false;

    void Update()
    {
        // 단순 이동만 담당 (판정은 NoteInputHandler 에서)
        if (!isHit)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isHit) return;

        if (other.CompareTag("CutLine"))
        {
            isHit = true;
            InGameUIManager.Instance.OnMissScoreOnly();
            Destroy(gameObject);
        }
    }
}
