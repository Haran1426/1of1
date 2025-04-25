using UnityEngine;

public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("CutLine"))
        {

            Destroy(gameObject);
        }
    }
}
