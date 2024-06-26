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
    

    // Update is called once per frame
    void Update()
    {

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
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJumping = false;
        }
    }
}
    


