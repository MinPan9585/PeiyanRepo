using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maincamera : MonoBehaviour
{
    //相机跟随player移动
    public float rotationSpeed = 5f; // 相机旋转速度
    public Transform target; // 运动物体的Transform组件
    public Vector3 offset; // 相机与物体的偏移量

    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z-5);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2)) // 检测鼠标中键按下
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // 根据鼠标输入旋转相机
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.Self);
        }
    }
}
