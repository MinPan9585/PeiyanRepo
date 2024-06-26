using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{
    public float maxJumpTime = 1f; // 最大跳跃时间
    public float maxEnergyBarWidth = 200f; // 能量条的最大宽度

    private Image energyBarImage;
    private float currentJumpTime = 0f;
    void Start()
    {
        energyBarImage = GetComponent<Image>();
        energyBarImage.fillAmount = 0f; // 初始能量条长度为0
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            currentJumpTime += Time.deltaTime;

            // 根据按键持续时间计算能量条长度
            float fillAmount = Mathf.Clamp01(currentJumpTime / maxJumpTime);
            energyBarImage.fillAmount = fillAmount;

            // 根据能量条长度设置能量条宽度
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

