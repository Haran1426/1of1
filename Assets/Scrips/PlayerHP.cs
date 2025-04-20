using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int hp = 100;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("플레이어 HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("사망");
            // 사망 처리
        }
    }
}
