using UnityEngine;

public class Damage : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        // 씬에 있는 Player 컴포넌트 찾기
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
            Destroy(collision.gameObject); // 노트 제거
        }
    }
}
