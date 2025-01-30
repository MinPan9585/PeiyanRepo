using UnityEngine;

public class CharacterOrientationController : MonoBehaviour
{
    public Transform handTransform; // �ֱ��� Transform ���
    public Transform characterTransform; // ��ɫ�� Transform ���

    private void Start()
    {
  
    }

    void Update()
    {
        if (handTransform != null && characterTransform != null)
        {
            // ��ȡ�ֱ��ĳ�������
            Vector3 handForward = handTransform.forward;

            // ���ֱ����������� Y ��������Ϊ 0�����Դ�ֱ����ĳ���
            handForward.y = 0f;

            // ���������й�һ������ʹ�䳤��Ϊ 1
            handForward.Normalize();

            // ���ý�ɫ�ĳ����봦�����ֱ�ˮƽ����һ��
            characterTransform.forward = handForward;
        }
    }
}