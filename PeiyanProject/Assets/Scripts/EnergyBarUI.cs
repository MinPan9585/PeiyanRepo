using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    public float maxJumpTime = 1f; // �����Ծʱ��
    public float maxEnergyBarWidth = 200f; // �������������

    private Image energyBarImage;
    private float currentJumpTime = 0f;
    void Start()
    {
        energyBarImage = GetComponent<Image>();
        energyBarImage.fillAmount = 0f; // ��ʼ����������Ϊ0
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            currentJumpTime += Time.deltaTime;

            // ���ݰ�������ʱ���������������
            float fillAmount = Mathf.Clamp01(currentJumpTime / maxJumpTime);
            energyBarImage.fillAmount = fillAmount;

            // ���������������������������
            float energyBarWidth = fillAmount * maxEnergyBarWidth;
            energyBarImage.rectTransform.sizeDelta = new Vector2(energyBarWidth, energyBarImage.rectTransform.sizeDelta.y);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            currentJumpTime = 0f;
            energyBarImage.fillAmount = 0f;
            energyBarImage.rectTransform.sizeDelta = new Vector2(0f, energyBarImage.rectTransform.sizeDelta.y);
        }
    }
}

