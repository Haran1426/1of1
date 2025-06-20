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
        Debug.Log("Animator �����: " + animator);
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
        // ���� Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                // �ٷ� ����
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else
            {
                // ���� ���̸� ��⿭�� ����
                attackQueued = true;
            }
        }

        // �ִϸ��̼��� idle�� ���ƿ��� ��
        if (isAttacking && state.IsName("idle"))
        {
            if (attackQueued)
            {
                // ��ٸ��� �Է� ����
                animator.SetTrigger("Attack");
                attackQueued = false;
            }
            else
            {
                // �ƹ��͵� ��� ���� ������ ���� ���� ����
                isAttacking = false;
            }
        }
    }
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("�÷��̾� HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("���");
        }
    }
}
