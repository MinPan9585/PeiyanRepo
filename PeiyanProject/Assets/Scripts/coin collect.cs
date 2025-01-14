using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class coincollect : MonoBehaviour
{
    public int coinCount = 0; // 金币数量
    
    public TMP_Text coinText;
    void Start()
    {
        // 初始化金币数量显示
        UpdateCoinText();
    }

    void OnTriggerEnter(Collider other)
    {
        // 检查触发的对象是否为金币
        if (other.gameObject.CompareTag("Coin"))
        {
            // 增加金币数量
            coinCount++;

            // 更新UI显示
            UpdateCoinText();

            // 销毁金币对象
            Destroy(other.gameObject);
        }
    }

    void UpdateCoinText()
    {
        // 更新UI文本显示
        coinText.text = "Coins: " + coinCount;
    }
}
