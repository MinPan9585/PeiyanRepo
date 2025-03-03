using UnityEngine;
using System.Collections;

public class MultiObjectController : MonoBehaviour
{
    // ���������
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;

    // ÿ������������߶�
    public float riseDistance1 ;
    public float riseDistance2 ;
    public float riseDistance3;
    public float riseDistance4;

    // ÿ�����������ʱ�䣨�룩
    public float riseTime1 ;
    public float riseTime2 ;
    public float riseTime3 ;
    public float riseTime4;



    // ����ĳ�ʼλ��
    private Vector3 originalPosition1;
    private Vector3 originalPosition2;
    private Vector3 originalPosition3;
    private Vector3 originalPosition4;

    private void Start()
    {
        // ����ÿ������ĳ�ʼλ��
        originalPosition1 = object1.transform.position;
        originalPosition2 = object2.transform.position;
        originalPosition3 = object3.transform.position;
        originalPosition4 = object4.transform.position;
        // ����ÿ�����������Э��
        StartCoroutine(RiseRoutine(object1, riseDistance1, riseTime1, originalPosition1));
        StartCoroutine(RiseRoutine(object2, riseDistance2, riseTime2, originalPosition2));
        StartCoroutine(RiseRoutine(object3, riseDistance3, riseTime3, originalPosition3));
        StartCoroutine(RiseRoutine(object4, riseDistance4, riseTime4, originalPosition4));
    }

    private void Update()
    {
        // ������Ը�����Ҫ��������߼�
    }

    // ����������Э��
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
        obj.transform.position = targetPosition; // ȷ�����յ���Ŀ��λ��
    }

    // ����ɫ�Ƿ�վ�������ϣ����Ը�����Ҫ��չΪ������壩
  

 

}