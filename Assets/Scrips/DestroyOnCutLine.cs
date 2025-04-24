using UnityEngine;

public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("충돌: {other.gameObject.name}");

        if (other.CompareTag("CutLine"))
        {
            Debug.Log("CutLine");

            Destroy(gameObject);
        }
    }
}
