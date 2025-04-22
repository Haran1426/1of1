using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCutLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("CutLine에서 충돌 감지: " + other.gameObject.name);
        }





    }
}
