using UnityEngine;

public class Player : MonoBehaviour
{
    
    private int HP;

    private void Awake()
    {
        HP = 100;
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
       
    }
    public void Damage(int amount)
    {
        HP -= amount;
        Debug.Log("�÷��̾� HP: " + HP);

        if (HP <= 0)
        {
            Debug.Log("���");
        }
    }
}
