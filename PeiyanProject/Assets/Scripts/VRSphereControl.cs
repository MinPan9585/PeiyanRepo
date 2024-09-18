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

/*������Щ�����Ѿ���������ж������  
float baseForce = 10f; // ������  
float horizontalForceMax = 50f; // ˮƽ�������ֵ  
float gripValue = ...; // ��ĳ����ȡ���ճ�ֵ����Χ������0��1  
bool isJumping = ...; // ��ĳ����ȡ����Ծ״̬  

// ����ˮƽ��  
if (isJumping)
{
    // ʹ��gripValue�����Ż���������������horizontalForceMax  
    float scaledForce = Mathf.Clamp(gripValue * (horizontalForceMax - baseForce) + baseForce, baseForce, horizontalForceMax);
    // �����Ҫ�������Գ���һ���������ϵ��  
    float finalForce = scaledForce * someForceMultiplier; // someForceMultiplier����һ�����ܴ��ڵ���ϵ��  

    // �������������ֵ��horizontalForce���������Ѿ���������ж��壩  
    horizontalForce = finalForce;
}
else
{
    // ���������Ծ״̬�����Խ�horizontalForce����Ϊ0������Ĭ��ֵ  
    horizontalForce = 0f;
}
*/