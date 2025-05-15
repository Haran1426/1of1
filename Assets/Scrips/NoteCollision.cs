using UnityEngine;

public class NoteCollision : MonoBehaviour
{
    public void Update()
    {
        Debug.Log(FindAnyObjectByType<NoteCollision>().name);
    }
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
