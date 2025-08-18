using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAroundZ : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // 旋转速度（单位：度/秒）

    void Update()
    {
        // 每帧旋转一定角度
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}