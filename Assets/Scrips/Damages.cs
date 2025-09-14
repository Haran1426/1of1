using UnityEngine;

public class Damage : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        // ���� �ִ� Player ������Ʈ ã��
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (player != null)
            {
                player.Damage(20);
            }
            Destroy(collision.gameObject); // ��Ʈ ����
        }
    }
}
