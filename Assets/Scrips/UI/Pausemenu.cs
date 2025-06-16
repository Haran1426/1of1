using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pausemenu : MonoBehaviour
{
    public GameObject pausePannel;
    public GameObject Target;
    public void Menu_Btn()
    {

        Time.timeScale = 0f;


        pausePannel.SetActive(true);
    }

    public void Continue()
    {

        Time.timeScale = 1f;
        pausePannel.SetActive(false);
        Target.gameObject.SetActive(true);
    }
}