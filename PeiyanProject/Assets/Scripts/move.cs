using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 5.0f; // 移动速度
    public Vector3 direction = Vector3.right; // 移动方向，初始为向右
    public float duration = 5.0f; // 单程移动时间

    private Vector3 startPosition; // 初始位置
    private Vector3 endPosition; // 目标位置
    private bool movingForward = true; // 是否向前移动
    private float journeyTime = 0f; // 已经旅行的时间
    private float journeyDistance = 0f; // 需要旅行的总距离






    void Start()
    {
        startPosition = transform.position; // 记录初始位置
        endPosition = startPosition + direction; // 计算目标位置
        journeyDistance = Vector3.Distance(startPosition, endPosition); // 计算总距离
        journeyTime = 0f; // 初始化已经旅行的时间
    }

    void Update()
    {
        // 根据移动方向计算当前位置
        if (movingForward)
        {
            // 从开始位置到结束位置插值
            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(journeyTime / duration));
        }
        else
        {
            // 从结束位置到开始位置插值
            transform.position = Vector3.Lerp(endPosition, startPosition, Mathf.Clamp01(journeyTime / duration));

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
    private void OnTriggerEnter(Collider other) {
        
        other.transform.SetParent(transform);

        
    }
    void OnTriggerExit(Collider other) { 
    other.transform.SetParent(null);}
}