using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 체력
    private int HP;

    // 애니메이터 컴포넌트 참조
    Animator animator;

    // 공격 애니메이션 상태 플래그
    bool isAttacking = false;

    // 공격 입력 대기 플래그
    bool attackQueued = false;

    private void Start()
    {
        // 초기 체력 설정
        HP = 100;
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 플레이어의 x 위치를 고정(-4.3f)
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        // P, K, M 키로 y 위치(라인) 변경
        if (Input.GetKeyDown(KeyCode.P))
        {
            // 상단 라인으로 이동
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // 중간 라인으로 이동
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // 하단 라인으로 이동
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        // R/G/B 키 입력 시 공격 트리거
        if (Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

        // 현재 애니메이터 상태 정보 가져오기
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // 스페이스바 공격 입력 처리 (콤보 큐 기능)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                // 현재 공격 중이 아니면 즉시 공격
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else
            {
                // 공격 중이면 다음 공격을 대기열에 저장
                attackQueued = true;
            }
        }

        // 애니메이션이 idle 상태로 돌아왔을 때 처리
        if (isAttacking && state.IsName("idle"))
        {
            if (attackQueued)
            {
                // 대기 중인 공격이 있으면 실행
                animator.SetTrigger("Attack");
                attackQueued = false;
            }
            else
            {
                // 대기 중인 입력이 없으면 공격 상태 종료
                isAttacking = false;
            }
        }
    }

    // 외부에서 호출하는 데미지 처리 메서드
    public void Damage(int amount)
    {
        // HP 감소
        HP -= amount;
        Debug.Log("플레이어 HP: " + HP);

        // 체력이 0 이하가 되면 사망 로그 출력
        if (HP <= 0)
        {
            Debug.Log("사망");
        }
    }
}
