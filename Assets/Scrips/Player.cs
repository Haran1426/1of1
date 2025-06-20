using UnityEngine;

public class Player : MonoBehaviour
{
    
    private int HP;
    Animator animator;

    private void Start()
    {
        HP = 100;
        animator = GetComponent<Animator>();
        Debug.Log("Animator ¿¬°áµÊ: " + animator);
    }
    void Update()
    {
        transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.K)) 
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }

        else if (Input.GetKeyDown(KeyCode.M)) 
        {
            transform.position = new Vector3(transform.position.x, -1.5f, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Attack");
        }

    }
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("ÇÃ·¹ÀÌ¾î HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("»ç¸Á");
        }
    }
}
