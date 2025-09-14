using UnityEngine;
using UnityEngine.UI;

public class Skill_Cool : MonoBehaviour
{
    [SerializeField] private Image cooldownImage; // ��Ÿ�� �̹���
    [SerializeField] private float cooldownTime = 30f; // ��Ÿ�� �ð� (��)

    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    private void Start()
    {
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f; // ���� �� ä���� ���� ����
    }

    private void Update()
    {
        // �׽�Ʈ: �����̽��� ������ �� ��ų ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCooldown();
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownImage != null)
                cooldownImage.fillAmount = cooldownTimer / cooldownTime;

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                cooldownImage.fillAmount = 0f; // �� ������ ����
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f; // �� ���� �� Ǯ�� ä��
    }

    public bool IsReady()
    {
        return !isCooldown;
    }
}
