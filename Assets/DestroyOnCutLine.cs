using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"🚨 충돌 감지: {other.gameObject.name}");

        if (other.gameObject.CompareTag("CutLine")) // 태그 비교 방식 변경
        {
            Point pointScript = FindObjectOfType<Point>();
            if (pointScript != null)
            {
                pointScript.RemoveFromList(gameObject);
            }
            Debug.Log($"🗑️ {gameObject.name} 삭제됨!");
            Destroy(gameObject);
        }
    }
}
