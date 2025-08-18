using UnityEngine;

public class CylinderController : MonoBehaviour
{
    private Vector3 originalPosition; // 柱体的初始位置
    public float maxDropDistance = 5f; // 最大下降距离
    public float dropSpeed; // 下降速度
    public float recoverySpeed = 0.5f; // 恢复速度
    public GameObject object2; // 需要控制激活状态的物体2
    

    private float currentDropDistance = 0f; // 当前下降距离
    private bool isCharacterOnTop = false; // 是否有角色站在上面
 

    private void Start()
    {
        originalPosition = transform.position;
        if (object2 != null)
        {
            object2.SetActive(false); // 确保物体2初始状态为非激活
        }
        else
        {
            Debug.LogError("物体2未正确引用！");
        }
    }

    private void Update()
    {
        // 如果角色站在上面，柱体下降
        if (isCharacterOnTop)
        {
            currentDropDistance += dropSpeed * Time.deltaTime;
            currentDropDistance = Mathf.Clamp(currentDropDistance, 0, maxDropDistance);

         
                    object2.SetActive(true); // 激活物体2
               
       
        }
        // 如果角色不在上面，柱体恢复
        else
        {
            currentDropDistance -= recoverySpeed * Time.deltaTime;
            currentDropDistance = Mathf.Clamp(currentDropDistance, 0, maxDropDistance);
        }

        // 更新柱体的位置
        transform.position = originalPosition + new Vector3(0, -currentDropDistance, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            isCharacterOnTop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            isCharacterOnTop = false;
        }
    }
}