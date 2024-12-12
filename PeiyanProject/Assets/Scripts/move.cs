using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 5.0f; // �ƶ��ٶ�
    public Vector3 direction = Vector3.right; // �ƶ����򣬳�ʼΪ����
    public float duration = 5.0f; // �����ƶ�ʱ��

    private Vector3 startPosition; // ��ʼλ��
    private Vector3 endPosition; // Ŀ��λ��
    private bool movingForward = true; // �Ƿ���ǰ�ƶ�
    private float journeyTime = 0f; // �Ѿ����е�ʱ��
    private float journeyDistance = 0f; // ��Ҫ���е��ܾ���






    void Start()
    {
        startPosition = transform.position; // ��¼��ʼλ��
        endPosition = startPosition + direction; // ����Ŀ��λ��
        journeyDistance = Vector3.Distance(startPosition, endPosition); // �����ܾ���
        journeyTime = 0f; // ��ʼ���Ѿ����е�ʱ��
    }

    void Update()
    {
        // �����ƶ�������㵱ǰλ��
        if (movingForward)
        {
            // �ӿ�ʼλ�õ�����λ�ò�ֵ
            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(journeyTime / duration));
        }
        else
        {
            // �ӽ���λ�õ���ʼλ�ò�ֵ
            transform.position = Vector3.Lerp(endPosition, startPosition, Mathf.Clamp01(journeyTime / duration));

        }

        // �����Ѿ����е�ʱ��
        journeyTime += speed * Time.deltaTime;

        // ���Ѿ����е�ʱ�䳬������ʱ��ʱ����ת������������ʱ��
        if (journeyTime >= duration)
        {
            movingForward = !movingForward; // ��ת�ƶ�����
            journeyTime = 0f; // �����Ѿ����е�ʱ��
        }
    }
    private void OnTriggerEnter(Collider other) {
        
        other.transform.SetParent(transform);

        
    }
    void OnTriggerExit(Collider other) { 
    other.transform.SetParent(null);}
}