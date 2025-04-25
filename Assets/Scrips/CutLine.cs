using UnityEngine;

public class CutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Score.Instance != null)
            {
                int penalty = Random.Range(100, 201); 
                Score.Instance.AddScore(-penalty);
                Debug.Log("�ƶ��� �浹: -{penalty}��");
            }
        }
    }
}
