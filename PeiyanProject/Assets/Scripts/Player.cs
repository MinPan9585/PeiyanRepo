using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    Vector3 fireDir;
    Vector3 finalFireDir;
    Vector3 hitPos;
 
//��Ծ
    public float jumpForceMin = 5f; // ��С��Ծ����
    public float jumpForceMax = 10f; // �����Ծ����
    public float maxJumpTime = 1f; // �����Ծʱ��

    private float jumpForce = 0f;
    private float jumpTime = 0f;
    private bool isJumping = false;
    public Vector3 direction;

    // Update is called once per frame
    void Update()
    {

        //xiangliang
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
                Vector3 mouseToObjectA = transform.position  - hit.point;

                // ���������ʹ��������������Ӧ�Ĳ���
                Debug.Log("���λ��������A��������" + mouseToObjectA);
                direction = new Vector3(mouseToObjectA.x, 0, mouseToObjectA.z);          
             } //�ո�
          
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // ���ո������
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) // ���ո����ס
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping) // ���ո���ͷ�
        {
            jumpForce = Mathf.Lerp(jumpForceMin, jumpForceMax, jumpTime / maxJumpTime);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce+direction , ForceMode.Impulse);

            isJumping = false;
        }
    }
}
    


