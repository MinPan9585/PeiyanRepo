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

    public float jumpForceMin = 5f;
    public float jumpForceMax = 10f;
    public float maxJumpTime = 1f;

    private float jumpForce = 0f;
    private float jumpTime = 0f;
    private bool isJumping = false;
    public Vector3 direction;

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

                direction = new Vector3(mouseToObjectA.x, 0, mouseToObjectA.z);          
             }
          
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) // ¼ì²â¿Õ¸ñ¼ü°´ÏÂ
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping) // ¼ì²â¿Õ¸ñ¼ü°´×¡
        {
            jumpTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && isJumping) // ¼ì²â¿Õ¸ñ¼üÊÍ·Å
        {
            jumpForce = Mathf.Lerp(jumpForceMin, jumpForceMax, jumpTime / maxJumpTime);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce+direction , ForceMode.Impulse);

            isJumping = false;
        }
    }
}
    


