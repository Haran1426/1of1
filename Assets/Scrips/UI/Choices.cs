using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class YIS
{
    public GameObject Obj;
    public int index;
}
public class Choices : MonoBehaviour
{
    public List<YIS> list = new List<YIS>();
    public int HAN;
    public int move;
    public int index;
    void Start()
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index = i;
        }
    }

    void Butten_Lozic(int num)
    {
        num = move;

    }
    void Update()
    {

    }
}
