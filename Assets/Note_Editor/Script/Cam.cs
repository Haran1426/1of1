using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cam : MonoBehaviour
{
    private Camera cam;
    private float Cam_Scale;

    void Start()
    {
        cam = GetComponent<Camera>();
        // ✅ 시작할 때 카메라 사이즈 동기화
        Cam_Scale = cam.orthographicSize;
    }

    void CamZoomLojic(float Scale = 1f)
    {
        if (Input.mouseScrollDelta.y > 0)       // 휠 위 = 줌 인
        {
            Cam_Scale -= 1f * Scale;
        }
        else if (Input.mouseScrollDelta.y < 0)  // 휠 아래 = 줌 아웃
        {
            Cam_Scale += 1f * Scale;
        }

        // ✅ 최소값만 제한 (0.1 이하 방지)
        if (Cam_Scale < 1f) Cam_Scale = 1f;

        // ✅ 부드럽게 적용
        float zoomSpeed = 20f;
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            Cam_Scale,
            zoomSpeed * Time.deltaTime
        );
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
