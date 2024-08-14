using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRSphereControl : MonoBehaviour
{
    public InputActionProperty pinchAction;

    bool isJumping;
    float jumpTime;
    float maxJumpTime;
    float jumpForce;
    float jumpForceMin = 2;
    float jumpForceMax = 9;

    void Update()
    {
        float gripValue = pinchAction.action.ReadValue<float>();

        if (gripValue >= 0.1f && !isJumping)
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (gripValue >= 0.1f && isJumping)
        {
            jumpTime += Time.deltaTime;
        }

        if (gripValue <= 0.1f && isJumping)
        {
            jumpForce = Mathf.Lerp(jumpForceMin, jumpForceMax, jumpTime / maxJumpTime);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + Camera.main.transform.forward, ForceMode.Impulse);

            isJumping = false;
        }
    }
}
