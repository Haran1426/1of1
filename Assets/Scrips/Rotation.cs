using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Header("ȸ�� �ӵ� (�ʴ� ����)")]
    public float rotationSpeed = 90f; // 1�ʿ� 90��

    void Update()
    {
        // Z�� �������� ��� ȸ��
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
