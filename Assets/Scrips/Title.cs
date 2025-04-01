using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("ddd");
    }
    public void StartButton2()
    {
        SceneManager.LoadScene("Title");
        Debug.Log("dddddd");
    }
}
