using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // �ƶ�����Ĭ������
    public float moveDistance = 5.0f; // �ƶ�����
    public float moveDuration = 2.0f; // �����ƶ�ʱ��
    public float turnSpeed = 90.0f; // ת���ٶȣ�ÿ����ת�Ƕȣ�

    private Vector3 localStartPosition; // ����ڸ�����ĳ�ʼλ��
    private Vector3 localEndPosition; // ����ڸ������Ŀ��λ��
    private bool movingForward = true; // �Ƿ���ǰ�ƶ�
    private float elapsedTime = 0.0f; // �Ѿ�������ʱ��
    private Quaternion targetRotation; // Ŀ����ת

    void Start()
    {
        // ��ȡ����ڸ�����ĳ�ʼλ��
        localStartPosition = transform.localPosition;
        // ��������ڸ������Ŀ��λ��
        localEndPosition = localStartPosition + moveDirection.normalized * moveDistance;
    }

    void Update()
    {
        // �����Ѿ�������ʱ��
        elapsedTime += Time.deltaTime;

        // �����Ƿ���ǰ�ƶ������㵱ǰλ��
        if (movingForward)
        {
            // �ӳ�ʼλ�õ�Ŀ��λ�ò�ֵ
            transform.localPosition = Vector3.Lerp(localStartPosition, localEndPosition, elapsedTime / moveDuration);
        }
        else
        {
            // ��Ŀ��λ�õ���ʼλ�ò�ֵ
            transform.localPosition = Vector3.Lerp(localEndPosition, localStartPosition, elapsedTime / moveDuration);
        }

        // ����Ѿ�������ʱ�䳬������ʱ�䣬��ת��������ʱ��
        if (elapsedTime >= moveDuration)
        {
            // ��ת�ƶ�����
            movingForward = !movingForward;
            elapsedTime = 0.0f; // ����ʱ��

            // ����Ŀ����ת
            if (movingForward)
            {
                targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(-moveDirection.normalized);
            }

            // ƽ����ת��Ŀ�귽��
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}