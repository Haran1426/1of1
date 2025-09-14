using UnityEngine;
using UnityEngine.UI;  // HP�� UI ���

public class Player : MonoBehaviour
{
    // ü�� ����
    private int HP;
    private int maxHP = 100;

    // HP�� UI
    [SerializeField] private Slider hpSlider;

    // �ִϸ�����
    private Animator animator;

    // ���� �ִϸ��̼� ���� �÷���
    private bool isAttacking = false;
    private bool attackQueued = false;

    private void Start()
    {
        // �ʱ� ü�� ����
        HP = maxHP;

        animator = GetComponent<Animator>();

        // HP�� �ʱ�ȭ
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = HP;
        }
    }

    private void Update()
    {
        // �÷��̾��� X ��ġ ���� (-4.3f)
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        // --------------------
        // ���� �̵� (P/K/M)
        // --------------------
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ��� ����
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // �ߴ� ����
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // �ϴ� ����
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        // --------------------
        // RGB ����
        // --------------------
        if (Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.G) ||
            Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

        // --------------------
        // �����̽� �޺� ����
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
                // ���� ���� �� �� ���� ���� ����
                attackQueued = true;
            }
        }

        // idle ���·� ���ƿ��� �޺� ó��
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
    // ������ ó��
    // --------------------
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("�÷��̾� HP: " + HP);

        // HP�� ����
        if (hpSlider != null)
        {
            hpSlider.value = HP;
        }

        // ��� ó��
        if (HP <= 0)
        {
            Debug.Log("���");
            // TODO: ���ӿ��� ���� ���� �ڸ�
        }
    }
}
