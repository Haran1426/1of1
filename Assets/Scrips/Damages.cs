using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damages : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP player = other.GetComponent<PlayerHP>();
            if (player != null)
            {
                player.TakeDamage(10); 
            }
        }
    }
}
