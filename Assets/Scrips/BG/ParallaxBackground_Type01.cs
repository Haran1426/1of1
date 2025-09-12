using UnityEngine;

public class ParallaxBackground_Type01 : MonoBehaviour
{
    [SerializeField] private Transform target;          // 이어질 반대 타일(짝)
    [SerializeField] private float scrollAmount;        // 두 타일 간 간격(= 스프라이트 실제 폭). 0이면 자동 계산
    [SerializeField] private float moveSpeed = 3f;      // 이동 속도(기본 3)
    [SerializeField] private Vector3 moveDirection = Vector3.left; // 이동 방향(기본 왼쪽)

    private SpriteRenderer sr;

    private void Awake()
    {
        // (L15~20) scrollAmount 자동 계산: 값이 0 이하이면 SpriteRenderer.bounds로 폭 산출(스케일 포함)
        if (scrollAmount <= 0f)
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                scrollAmount = sr.bounds.size.x; // 스프라이트 실제 폭(스케일 포함)
        }

        // (L23~24) 방향 정규화: 속도 안정화
        if (moveDirection.sqrMagnitude > 0f)
            moveDirection = moveDirection.normalized;
    }

    private void Update()
    {
        // (L30) 배경 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // (L33) 안전장치: Target 미지정 시 반환
        if (target == null) return;

        // (L36~47) 래핑(무한 스크롤) — 항상 "Target 기준"으로 판단
        if (Mathf.Abs(moveDirection.x) > 0.0001f)
        {
            if (moveDirection.x < 0f) // 왼쪽 이동
            {
                if (transform.position.x <= target.position.x - scrollAmount)
                    transform.position = target.position + Vector3.right * scrollAmount;
            }
            else // 오른쪽 이동
            {
                if (transform.position.x >= target.position.x + scrollAmount)
                    transform.position = target.position - Vector3.right * scrollAmount;
            }
        }

        // (L51) 필요 시 Y축(세로 스크롤)도 동일한 방식으로 추가 가능
        // if (Mathf.Abs(moveDirection.y) > 0.0001f) { ... }
    }
}
