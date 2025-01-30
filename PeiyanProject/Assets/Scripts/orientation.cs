using UnityEngine;

public class CharacterOrientationController : MonoBehaviour
{
    public Transform handTransform; // 手柄的 Transform 组件
    public Transform characterTransform; // 角色的 Transform 组件

    private void Start()
    {
  
    }

    void Update()
    {
        if (handTransform != null && characterTransform != null)
        {
            // 获取手柄的朝向向量
            Vector3 handForward = handTransform.forward;

            // 将手柄朝向向量的 Y 分量设置为 0，忽略垂直方向的朝向
            handForward.y = 0f;

            // 对向量进行归一化处理，使其长度为 1
            handForward.Normalize();

            // 设置角色的朝向与处理后的手柄水平方向一致
            characterTransform.forward = handForward;
        }
    }
}