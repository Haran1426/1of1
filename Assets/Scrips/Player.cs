using UnityEngine;

public class Player : MonoBehaviour
{
    // �÷��̾� ü��
    private int HP;

    // �ִϸ����� ������Ʈ ����
    Animator animator;

    // ���� �ִϸ��̼� ���� �÷���
    bool isAttacking = false;

    // ���� �Է� ��� �÷���
    bool attackQueued = false;

    private void Start()
    {
        // �ʱ� ü�� ����
        HP = 100;
        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // �÷��̾��� x ��ġ�� ����(-4.3f)
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        // P, K, M Ű�� y ��ġ(����) ����
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ��� �������� �̵�
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // �߰� �������� �̵�
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // �ϴ� �������� �̵�
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        // R/G/B Ű �Է� �� ���� Ʈ����
        if (Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

        // ���� �ִϸ����� ���� ���� ��������
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // �����̽��� ���� �Է� ó�� (�޺� ť ���)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                // ���� ���� ���� �ƴϸ� ��� ����
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
            else
            {
                // ���� ���̸� ���� ������ ��⿭�� ����
                attackQueued = true;
            }
        }

        // �ִϸ��̼��� idle ���·� ���ƿ��� �� ó��
        if (isAttacking && state.IsName("idle"))
        {
            if (attackQueued)
            {
                // ��� ���� ������ ������ ����
                animator.SetTrigger("Attack");
                attackQueued = false;
            }
            else
            {
                // ��� ���� �Է��� ������ ���� ���� ����
                isAttacking = false;
            }
        }
    }

    // �ܺο��� ȣ���ϴ� ������ ó�� �޼���
    public void Damage(int amount)
    {
        // HP ����
        HP -= amount;
        Debug.Log("�÷��̾� HP: " + HP);

        // ü���� 0 ���ϰ� �Ǹ� ��� �α� ���
        if (HP <= 0)
        {
            Debug.Log("���");
        }
    }
}
