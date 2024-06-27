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
 
//跳跃
    public float jumpForceMin = 5f; // 最小跳跃力度
    public float jumpForceMax = 10f; // 最大跳跃力度
    public float maxJumpTime = 1f; // 最大跳跃时间

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

                // 在这里可以使用向量来进行相应的操作
                Debug.Log("鼠标位置与物体A的向量：" + mouseToObjectA);
                direction = new Vector3(mouseToObjectA.x, 0, mouseToObjectA.z);          
             } //空格
          
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // 检测空格键按下
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) // 检测空格键按住
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping) // 检测空格键释放
        {
            jumpForce = Mathf.Lerp(jumpForceMin, jumpForceMax, jumpTime / maxJumpTime);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce+direction , ForceMode.Impulse);

            isJumping = false;
        }
    }
}
    


