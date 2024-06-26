using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maincamera : MonoBehaviour
{
    //�������player�ƶ�
    public float rotationSpeed = 5f; // �����ת�ٶ�
    public Transform target; // �˶������Transform���
    public Vector3 offset; // ����������ƫ����

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
        if (Input.GetMouseButton(2)) // �������м�����
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // �������������ת���
            transform.Rotate(Vector3.up, mouseX * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, -mouseY * rotationSpeed, Space.Self);
        }
    }
}
