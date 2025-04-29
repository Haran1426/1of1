using UnityEngine;

public class NoteCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CutLine"))
        {
            Score.Instance.OnHitCutLine();
            Debug.Log("Note∞° CutLineø° ¥Í¿Ω");

        }
        else if (collision.CompareTag("HitBox"))
        {
            Score.Instance.OnHitHitBox();
            Debug.Log("Note∞° HitBoxø° ¥Í¿Ω");
            Destroy(gameObject);
        }
    }
}
