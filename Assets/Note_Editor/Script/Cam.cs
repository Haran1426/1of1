using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cam : MonoBehaviour
{
    private Camera cam;
    private float Cam_Scale;

    // ✅ 마우스 드래그 시작 위치
    private Vector3 dragOrigin;

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
        // 마우스 휠 버튼 눌렀을 때 시작 지점 기록
        if (Input.GetMouseButtonDown(2))  // 2 = 휠 버튼
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // 마우스 휠 버튼을 누르고 있을 때 카메라 이동
        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }
    }

    void Update()
    {
        Cam_Move();
        CamZoomLojic();
    }
}
