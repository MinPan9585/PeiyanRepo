using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRSphereControl : MonoBehaviour
{
    public InputActionProperty pinchAction;

    bool isJumping;
    float jumpTime;
    float maxJumpTime = 5;
    float jumpForce;
    float horizontalForce;
    float horizontalForceMin = 2;
    float horizontalForceMax = 9;

    public float heightMin = 0.5f;
    public float heightMax = 2.0f;
    float rightHandHeight;
    public Transform rightHand;

    void Update()
    {
        rightHandHeight = rightHand.position.y;
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
            jumpForce = Mathf.Lerp(heightMin, heightMax, rightHandHeight / heightMax);

            horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, jumpTime / horizontalForceMax);


            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + horizontalForce * rightHand.forward, ForceMode.Impulse);

            isJumping = false;
        }
    }
}
