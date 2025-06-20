using UnityEngine;

public class Player : MonoBehaviour
{
    
    private int HP;
    Animator animator;
    bool isAttacking = false;
    bool attackQueued = false;
    private void Start()
    {
        HP = 100;
        animator = GetComponent<Animator>();
        Debug.Log("Animator 연결됨: " + animator);
    }
    void Update()
    {
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K)) 
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }

        else if (Input.GetKeyDown(KeyCode.M)) 
        {
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        // 공격 키 입력 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                // 바로 공격
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else
            {
                // 공격 중이면 대기열에 저장
                attackQueued = true;
            }
        }

        // 애니메이션이 idle로 돌아왔을 때
        if (isAttacking && state.IsName("idle"))
        {
            if (attackQueued)
            {
                // 기다리던 입력 실행
                animator.SetTrigger("Attack");
                attackQueued = false;
            }
            else
            {
                // 아무것도 대기 중이 없으면 공격 상태 종료
                isAttacking = false;
            }
        }
    }
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("플레이어 HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("사망");
        }
    }
}
