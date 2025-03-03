using UnityEngine;
using System.Collections;

public class MultiObjectController : MonoBehaviour
{
    // 物体的引用
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;

    // 每个物体的上升高度
    public float riseDistance1 ;
    public float riseDistance2 ;
    public float riseDistance3;
    public float riseDistance4;

    // 每个物体的上升时间（秒）
    public float riseTime1 ;
    public float riseTime2 ;
    public float riseTime3 ;
    public float riseTime4;



    // 物体的初始位置
    private Vector3 originalPosition1;
    private Vector3 originalPosition2;
    private Vector3 originalPosition3;
    private Vector3 originalPosition4;

    private void Start()
    {
        // 保存每个物体的初始位置
        originalPosition1 = object1.transform.position;
        originalPosition2 = object2.transform.position;
        originalPosition3 = object3.transform.position;
        originalPosition4 = object4.transform.position;
        // 启动每个物体的上升协程
        StartCoroutine(RiseRoutine(object1, riseDistance1, riseTime1, originalPosition1));
        StartCoroutine(RiseRoutine(object2, riseDistance2, riseTime2, originalPosition2));
        StartCoroutine(RiseRoutine(object3, riseDistance3, riseTime3, originalPosition3));
        StartCoroutine(RiseRoutine(object4, riseDistance4, riseTime4, originalPosition4));
    }

    private void Update()
    {
        // 这里可以根据需要添加其他逻辑
    }

    // 物体上升的协程
    private IEnumerator RiseRoutine(GameObject obj, float riseDistance, float riseTime, Vector3 originalPosition)
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = originalPosition + Vector3.up * riseDistance;
        while (elapsedTime < riseTime)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position,
                targetPosition, riseDistance / riseTime * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = targetPosition; // 确保最终到达目标位置
    }

    // 检测角色是否站在物体上（可以根据需要扩展为多个物体）
  

 

}