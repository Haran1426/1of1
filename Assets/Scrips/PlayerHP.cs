using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int hp = 100;

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("�÷��̾� HP: " + hp);

        if (hp <= 0)
        {
            Debug.Log("���");
            // ��� ó��
        }
    }
}
