using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRSphereControl : MonoBehaviour
{
    public InputActionProperty pinchAction;

    public float force;
    bool isJumping;
    float jumpTime;
    float maxJumpTime = 5;
    float jumpForce;
    float horizontalForce;
    float horizontalForceMin = 2;
    float horizontalForceMax = 9;


    public float heightMin ;
    public float heightMax ;
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
            
            
            jumpForce = Mathf.Lerp(heightMin, heightMax, rightHandHeight / heightMax)*(rightHandHeight / heightMin);

            horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, jumpTime / horizontalForceMax) * force;


            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + horizontalForce * rightHand.forward, ForceMode.Impulse);

            isJumping = false;
        }
    }
}

/*假设这些变量已经在你的类中定义好了  
float baseForce = 10f; // 基础力  
float horizontalForceMax = 50f; // 水平力的最大值  
float gripValue = ...; // 从某处获取的握持值，范围可能是0到1  
bool isJumping = ...; // 从某处获取的跳跃状态  

// 计算水平力  
if (isJumping)
{
    // 使用gripValue来缩放基础力，但不超过horizontalForceMax  
    float scaledForce = Mathf.Clamp(gripValue * (horizontalForceMax - baseForce) + baseForce, baseForce, horizontalForceMax);
    // 如果需要，还可以乘以一个额外的力系数  
    float finalForce = scaledForce * someForceMultiplier; // someForceMultiplier是另一个可能存在的力系数  

    // 将计算出的力赋值给horizontalForce（假设它已经在你的类中定义）  
    horizontalForce = finalForce;
}
else
{
    // 如果不是跳跃状态，可以将horizontalForce设置为0或其他默认值  
    horizontalForce = 0f;
}
*/