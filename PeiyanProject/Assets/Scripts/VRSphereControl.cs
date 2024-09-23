using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VRSphereControl : MonoBehaviour
{
    public InputActionProperty pinchAction;
    public float force1;
    public float force2;
    bool isJumping;
    float jumpTime;
    public float maxJumpTime;
    float jumpForce;
    float horizontalForce;
    public float horizontalForceMin ;
    public float horizontalForceMax ;

    public float heightMin ;
    public float heightMax ;
    float rightHandHeight;
    public Transform rightHand;

    private float gripValue;
    public Image energyBarImage;

    public Transform arrow;

    void Update()
    {
        rightHandHeight = rightHand.position.y;
        gripValue = pinchAction.action.ReadValue<float>();
        arrow.forward = new Vector3(rightHand.forward.x, 0, rightHand.forward.z);


        if (gripValue >= 0.1f && !isJumping)
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (gripValue >= 0.1f && isJumping)
        {
            jumpTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(jumpTime / maxJumpTime);
            energyBarImage.fillAmount = fillAmount;
            horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, jumpTime / horizontalForceMax) * force2;
            jumpForce = Mathf.Lerp(0, 1, (rightHandHeight - heightMin) / (heightMax - heightMin)) * force1;
            arrow.forward = Vector3.up * jumpForce + horizontalForce * new Vector3(rightHand.forward.x, 0, rightHand.forward.z);
        }

        if (gripValue <= 0.1f && isJumping)
        {
            
            energyBarImage.fillAmount = 0f;

            jumpForce = Mathf.Lerp(0, 1, (rightHandHeight - heightMin) / (heightMax - heightMin)) * force1;
            //Debug.Log(jumpForce);
            

            horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, jumpTime / horizontalForceMax) * force2;

            //Debug.Log(horizontalForce);
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce + horizontalForce * new Vector3(rightHand.forward.x,0, rightHand.forward.z), ForceMode.Impulse);
            arrow.forward = Vector3.up * jumpForce + horizontalForce * new Vector3(rightHand.forward.x, 0, rightHand.forward.z);

            jumpTime = 0f;
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