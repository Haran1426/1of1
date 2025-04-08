using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Slider HPbar;
    private float maxHP = 100;
    private float curHP = 100;
    float imsi;

    void Start()
    {
        imsi = curHP / maxHP;
        HPbar.value = imsi;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curHP > 0)
            {
                curHP -= 10;
                if (curHP < 0) curHP = 0;
            }

            imsi = curHP / maxHP; 
        }

        HandleHP();
    }

    private void HandleHP()
    {
        HPbar.value = Mathf.Lerp(HPbar.value, imsi, Time.deltaTime * 10);
    }
}
