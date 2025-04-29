using UnityEngine;

public class NoteCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CutLine"))
        {
            Score.Instance.OnHitCutLine();


        }
        else if (collision.CompareTag("HitBox"))
        {
            Score.Instance.OnHitHitBox();
            Destroy(gameObject);
        }

    }





}
