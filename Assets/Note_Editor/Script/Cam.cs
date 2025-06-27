using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cam : MonoBehaviour
{
    private Camera cam;
    private int Cam_Scale;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void CamZoomLojic(int Scale = 1)
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Cam_Scale -= 1 * Scale;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Cam_Scale += 1 * Scale;
        }

        if (Cam_Scale < 5)
        {
            Cam_Scale = 5 * Scale;
        }
        else if (Cam_Scale > 15 * Scale)
        {
            Cam_Scale = 15 * Scale;
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Cam_Scale, 4 * Scale * Time.deltaTime);
    }
    void Cam_Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //start_Point = 
        }
    }
    void Update()
    {
        Cam_Move();
        CamZoomLojic();
    }
}
