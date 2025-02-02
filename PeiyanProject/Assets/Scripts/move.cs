using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5.0f; // 移动速度
    public Vector3 direction = Vector3.right; // 移动方向，初始为向右
    public float duration = 5.0f; // 单程移动时间

    private Vector3 localStartPosition; // 相对于父物体的初始位置
    private Vector3 localEndPosition; // 相对于父物体的目标位置
    private bool movingForward = true; // 是否向前移动
    private float journeyTime = 0f; // 已经旅行的时间
    private float journeyDistance = 0f; // 需要旅行的总距离

    void Start()
    {
        localStartPosition = transform.localPosition; // 记录相对于父物体的初始位置
        localEndPosition = localStartPosition + direction; // 计算相对于父物体的目标位置
        journeyDistance = Vector3.Distance(localStartPosition, localEndPosition); // 计算总距离
        journeyTime = 0f; // 初始化已经旅行的时间
    }

    void Update()
    {
        // 根据移动方向计算当前位置
        if (movingForward)
        {
            // 从开始位置到结束位置插值
            transform.localPosition = Vector3.Lerp(localStartPosition, localEndPosition, Mathf.Clamp01(journeyTime / duration));
        }
        else
        {
            // 从结束位置到开始位置插值
            transform.localPosition = Vector3.Lerp(localEndPosition, localStartPosition, Mathf.Clamp01(journeyTime / duration));
        }

        // 更新已经旅行的时间
        journeyTime += speed * Time.deltaTime;

        // 当已经旅行的时间超过单程时间时，反转方向并重置旅行时间
        if (journeyTime >= duration)
        {
            movingForward = !movingForward; // 反转移动方向
            journeyTime = 0f; // 重置已经旅行的时间
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}