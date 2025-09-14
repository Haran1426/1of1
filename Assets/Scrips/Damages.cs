using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[CutLine] 충돌: " + other.name);

        if (other.CompareTag("Note"))
        {
            if (player != null)
            {
                player.Damage(damage);
                Debug.Log("[CutLine] HP 감소 → " + damage);
            }
            Destroy(other.gameObject);
        }
    }
}
