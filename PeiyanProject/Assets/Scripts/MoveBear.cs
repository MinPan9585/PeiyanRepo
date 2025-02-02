using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // 移动方向，默认向右
    public float moveDistance = 5.0f; // 移动距离
    public float moveDuration = 2.0f; // 单程移动时间
    public float turnSpeed = 90.0f; // 转身速度（每秒旋转角度）

    private Vector3 localStartPosition; // 相对于父物体的初始位置
    private Vector3 localEndPosition; // 相对于父物体的目标位置
    private bool movingForward = true; // 是否向前移动
    private float elapsedTime = 0.0f; // 已经经过的时间
    private Quaternion targetRotation; // 目标旋转

    void Start()
    {
        // 获取相对于父物体的初始位置
        localStartPosition = transform.localPosition;
        // 计算相对于父物体的目标位置
        localEndPosition = localStartPosition + moveDirection.normalized * moveDistance;
    }

    void Update()
    {
        // 更新已经经过的时间
        elapsedTime += Time.deltaTime;

        // 根据是否向前移动来计算当前位置
        if (movingForward)
        {
            // 从初始位置到目标位置插值
            transform.localPosition = Vector3.Lerp(localStartPosition, localEndPosition, elapsedTime / moveDuration);
        }
        else
        {
            // 从目标位置到初始位置插值
            transform.localPosition = Vector3.Lerp(localEndPosition, localStartPosition, elapsedTime / moveDuration);
        }

        // 如果已经经过的时间超过单程时间，反转方向并重置时间
        if (elapsedTime >= moveDuration)
        {
            // 反转移动方向
            movingForward = !movingForward;
            elapsedTime = 0.0f; // 重置时间

            // 计算目标旋转
            if (movingForward)
            {
                targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(-moveDirection.normalized);
            }

            // 平滑旋转到目标方向
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}