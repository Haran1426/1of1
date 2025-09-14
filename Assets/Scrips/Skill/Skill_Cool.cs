using UnityEngine;
using UnityEngine.UI;

public class Skill_Cool : MonoBehaviour
{
    [SerializeField] private Image cooldownImage; // 쿨타임 이미지
    [SerializeField] private float cooldownTime = 30f; // 쿨타임 시간 (초)

    private float cooldownTimer = 0f;
    private bool isCooldown = false;

    private void Start()
    {
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f; // 시작 시 채워져 있지 않음
    }

    private void Update()
    {
        // 테스트: 스페이스바 눌렀을 때 스킬 사용
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
                cooldownImage.fillAmount = 0f; // 다 끝나면 꺼짐
            }
        }
    }

    public void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 1f; // 쿨 시작 시 풀로 채움
    }

    public bool IsReady()
    {
        return !isCooldown;
    }
}
