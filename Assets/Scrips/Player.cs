using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ===== HP 관련 =====
    private int HP;
    private int maxHP = 100;

    [SerializeField] private Slider hpSlider;   // HP바 UI
    [SerializeField] private GameObject gameOverPanel; // 🆕 게임오버 패널

    // ===== 애니메이션 관련 =====
    private Animator animator;
    private bool isAttacking = false;
    private bool attackQueued = false;

    private void Start()
    {
        HP = maxHP;
        animator = GetComponent<Animator>();

        if (hpSlider != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = maxHP;
            hpSlider.value = HP;
        }

        // 시작할 땐 패널 숨김
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // X 위치 고정
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        // 라인 이동 (P/K/M)
        if (Input.GetKeyDown(KeyCode.P))
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        else if (Input.GetKeyDown(KeyCode.K))
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        else if (Input.GetKeyDown(KeyCode.M))
            transform.position = new Vector3(transform.position.x, -1f, transform.position.z);

        // RGB 공격
        if (Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

        // 스페이스 콤보 공격
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
                attackQueued = true;
            }
        }

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

    // ===== 데미지 처리 =====
    public void Damage(int amount)
    {
        HP -= amount;
        if (HP < 0) HP = 0;

        Debug.Log("플레이어 HP: " + HP);

        if (hpSlider != null)
            hpSlider.value = HP;

        if (HP <= 0)
        {
            Debug.Log("사망");
            ShowGameOverPanel(); // 🆕 패널 표시
        }
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 게임 멈추기
        Time.timeScale = 0f;
    }

}
