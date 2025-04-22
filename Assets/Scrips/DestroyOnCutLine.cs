using UnityEngine;

public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log($"충돌 감지: {other.gameObject.name}");

        if (other.CompareTag("CutLine"))
        {
            Debug.Log("🔴 CutLine과 충돌!");

            Destroy(gameObject);
        }
    }
}
