using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Header("회전 속도 (초당 각도)")]
    public float rotationSpeed = 90f; // 1초에 90도

    void Update()
    {
        // Z축 기준으로 계속 회전
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
