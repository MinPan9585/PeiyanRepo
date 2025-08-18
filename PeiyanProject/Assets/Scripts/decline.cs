using UnityEngine;

public class CylinderController : MonoBehaviour
{
    private Vector3 originalPosition; // ����ĳ�ʼλ��
    public float maxDropDistance = 5f; // ����½�����
    public float dropSpeed; // �½��ٶ�
    public float recoverySpeed = 0.5f; // �ָ��ٶ�
    public GameObject object2; // ��Ҫ���Ƽ���״̬������2
    

    private float currentDropDistance = 0f; // ��ǰ�½�����
    private bool isCharacterOnTop = false; // �Ƿ��н�ɫվ������
 

    private void Start()
    {
        originalPosition = transform.position;
        if (object2 != null)
        {
            object2.SetActive(false); // ȷ������2��ʼ״̬Ϊ�Ǽ���
        }
        else
        {
            Debug.LogError("����2δ��ȷ���ã�");
        }
    }

    private void Update()
    {
        // �����ɫվ�����棬�����½�
        if (isCharacterOnTop)
        {
            currentDropDistance += dropSpeed * Time.deltaTime;
            currentDropDistance = Mathf.Clamp(currentDropDistance, 0, maxDropDistance);

         
                    object2.SetActive(true); // ��������2
               
       
        }
        // �����ɫ�������棬����ָ�
        else
        {
            currentDropDistance -= recoverySpeed * Time.deltaTime;
            currentDropDistance = Mathf.Clamp(currentDropDistance, 0, maxDropDistance);
        }

        // ���������λ��
        transform.position = originalPosition + new Vector3(0, -currentDropDistance, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            isCharacterOnTop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            isCharacterOnTop = false;
        }
    }
}