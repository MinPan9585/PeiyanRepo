using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5.0f; // �ƶ��ٶ�
    public Vector3 direction = Vector3.right; // �ƶ����򣬳�ʼΪ����
    public float duration = 5.0f; // �����ƶ�ʱ��

    private Vector3 localStartPosition; // ����ڸ�����ĳ�ʼλ��
    private Vector3 localEndPosition; // ����ڸ������Ŀ��λ��
    private bool movingForward = true; // �Ƿ���ǰ�ƶ�
    private float journeyTime = 0f; // �Ѿ����е�ʱ��
    private float journeyDistance = 0f; // ��Ҫ���е��ܾ���

    void Start()
    {
        localStartPosition = transform.localPosition; // ��¼����ڸ�����ĳ�ʼλ��
        localEndPosition = localStartPosition + direction; // ��������ڸ������Ŀ��λ��
        journeyDistance = Vector3.Distance(localStartPosition, localEndPosition); // �����ܾ���
        journeyTime = 0f; // ��ʼ���Ѿ����е�ʱ��
    }

    void Update()
    {
        // �����ƶ�������㵱ǰλ��
        if (movingForward)
        {
            // �ӿ�ʼλ�õ�����λ�ò�ֵ
            transform.localPosition = Vector3.Lerp(localStartPosition, localEndPosition, Mathf.Clamp01(journeyTime / duration));
        }
        else
        {
            // �ӽ���λ�õ���ʼλ�ò�ֵ
            transform.localPosition = Vector3.Lerp(localEndPosition, localStartPosition, Mathf.Clamp01(journeyTime / duration));
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

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}