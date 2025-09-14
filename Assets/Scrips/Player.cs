using UnityEngine;
using UnityEngine.UI;  // HP바 UI 사용

public class Player : MonoBehaviour
{
    // 체력 관련
    private int HP;
    private int maxHP = 100;

    // HP바 UI
    [SerializeField] private Slider hpSlider;

    // 애니메이터
    private Animator animator;

    // 공격 애니메이션 상태 플래그
    private bool isAttacking = false;
    private bool attackQueued = false;

    private void Start()
    {
        // 초기 체력 세팅
        HP = maxHP;

        animator = GetComponent<Animator>();

        // HP바 초기화
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = HP;
        }
    }

    private void Update()
    {
        // 플레이어의 X 위치 고정 (-4.3f)
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        // --------------------
        // 라인 이동 (P/K/M)
        // --------------------
        if (Input.GetKeyDown(KeyCode.P))
        {
            // 상단 라인
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // 중단 라인
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // 하단 라인
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        // --------------------
        // RGB 공격
        // --------------------
        if (Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

        // --------------------
        // 스페이스 콤보 공격
        // --------------------
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else
            {
                // 현재 공격 중 → 다음 공격 예약
                attackQueued = true;
            }
        }

        // idle 상태로 돌아오면 콤보 처리
        if (isAttacking && state.IsName("idle"))
        {
            if (attackQueued)
            {
                animator.SetTrigger("Attack");
                attackQueued = false;
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    // --------------------
    // 데미지 처리
    // --------------------
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("플레이어 HP: " + HP);

        // HP바 갱신
        if (hpSlider != null)
        {
            hpSlider.value = HP;
        }

        // 사망 처리
        if (HP <= 0)
        {
            Debug.Log("사망");
            // TODO: 게임오버 로직 넣을 자리
        }
    }
}
