using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAroundZ : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // ��ת�ٶȣ���λ����/�룩

    void Update()
    {
        // ÿ֡��תһ���Ƕ�
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}