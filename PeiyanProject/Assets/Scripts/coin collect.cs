using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class coincollect : MonoBehaviour
{
    public int coinCount = 0; // �������
    
    public TMP_Text coinText;
    void Start()
    {
        // ��ʼ�����������ʾ
        UpdateCoinText();
    }

    void OnTriggerEnter(Collider other)
    {
        // ��鴥���Ķ����Ƿ�Ϊ���
        if (other.gameObject.CompareTag("Coin"))
        {
            // ���ӽ������
            coinCount++;

            // ����UI��ʾ
            UpdateCoinText();

            // ���ٽ�Ҷ���
            Destroy(other.gameObject);
        }
    }

    void UpdateCoinText()
    {
        // ����UI�ı���ʾ
        coinText.text = "Coins: " + coinCount;
    }
}
