using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class VRSphereControl : MonoBehaviour
{
    public InputActionProperty triggerAction;
    public float maxJumpHeight = 5f;
    public float maxJumpDistance = 10f;
    public float maxChargeTime = 2f;
    public Transform rightHand;
    public Transform chara;

    public Image energyBarImage;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 20;
    public float gravity = -9.81f;
    public float trajectoryLineWidth = 0.1f;

    // 喷射烟雾特效 GameObject 引用
    public GameObject jumpEffect;
    // 爆炸特效预制体
    public GameObject explosionEffectPrefab;

    // 喷射烟雾特效持续时间
    public float effectDuration = 1f;
    // 爆炸特效持续时间
    public float explosionDuration = 1f;

    // 新增燃料相关变量
    public int maxFuel = 20; // 最大燃料量
    public int currentFuel; // 当前燃料量
    public TMP_Text fuelText;   // 显示燃料量的 UI 文本组件

    // 触碰音效
    public AudioClip touchSoundClip; // 触碰燃料的音效剪辑
    private AudioSource audioSource; // 触碰燃料的音效源组件

    private float chargeTime;
    private bool isCharging;
    private Vector3 jumpDirection;

    // 新增变量：是否触碰了“yanjiang”标签的物体
    private bool isTouchingYanjiang = false;
    private float yanjiangTimer;
   //火焰特效
   public GameObject fire;


    void Start()
    {
        // 初始化燃料
        currentFuel = maxFuel;

        // 游戏开始时禁用喷射烟雾特效
        if (jumpEffect != null)
        {
            jumpEffect.SetActive(false);
        }

        // 更新燃料显示
        UpdateFuelText();
        fire.SetActive(false);
    }

    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();

        if (triggerValue > 0.1f && !isCharging)
        {
            chargeTime = 0f;
            isCharging = true;
        }

        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);

            energyBarImage.fillAmount = chargeTime / maxChargeTime;

            jumpDirection = rightHand.forward;
            jumpDirection.y = 0f;
            jumpDirection.Normalize();

            DrawTrajectory();

            if (triggerValue <= 0.1f)
            {
                Jump();
            }
        }
        else
        {
            trajectoryLine.enabled = false;
        }
        if (isTouchingYanjiang)
        {
            yanjiangTimer += Time.deltaTime;
            if (yanjiangTimer >= 2f)
            {
                yanjiangTimer = 0f; // 重置计时器
                currentFuel--; // 每2秒减少1点燃料
                currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); // 确保燃料不小于0
                UpdateFuelText(); // 更新燃料显示
            }
        }

    }

    void Jump()
    {
        if (currentFuel <= 0) return; // 如果燃料耗尽，直接返回

        float chargeRatio = chargeTime / maxChargeTime;
        float jumpForce = Mathf.Sqrt(-2f * gravity * maxJumpHeight * chargeRatio);
        float horizontalForce = Mathf.Sqrt(2f * maxJumpDistance * chargeRatio * -gravity);

        Vector3 jumpVector = jumpDirection * horizontalForce + Vector3.up * jumpForce;

        GetComponent<Rigidbody>().AddForce(jumpVector, ForceMode.Impulse);

        isCharging = false;
        chargeTime = 0f;
        energyBarImage.fillAmount = 0f;

        // 消耗燃料
        currentFuel--;
        UpdateFuelText();

        // 播放跳跃音效
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 激活喷射烟雾特效并调整方向
        if (jumpEffect != null)
        {
            jumpEffect.transform.forward = -jumpVector.normalized;
            jumpEffect.SetActive(true);
            StartCoroutine(DisableEffectAfterTime(jumpEffect, effectDuration));
        }

        // 生成爆炸特效
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffectInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            StartCoroutine(DestroyEffectAfterTime(explosionEffectInstance, explosionDuration));
        }
    }

    void DrawTrajectory()
    {
        trajectoryLine.enabled = true;
        trajectoryLine.startWidth = trajectoryLineWidth;
        trajectoryLine.endWidth = trajectoryLineWidth;

    

        float chargeRatio = chargeTime / maxChargeTime;
        float jumpForce = Mathf.Sqrt(-2f * gravity * maxJumpHeight * chargeRatio);
        float horizontalForce = Mathf.Sqrt(2f * maxJumpDistance * chargeRatio * -gravity);

        Vector3 initialVelocity = jumpDirection * horizontalForce + Vector3.up * jumpForce;

        float totalTime = 2f * jumpForce / -gravity;

        trajectoryLine.positionCount = trajectoryPoints;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = (float)i / (trajectoryPoints - 1) * totalTime;
            Vector3 position = chara.position + initialVelocity * t + 0.5f * new Vector3(0, gravity, 0) * t * t;
            trajectoryLine.SetPosition(i, position);
        }
    }

    // 协程用于控制喷射烟雾特效显示时间
    IEnumerator DisableEffectAfterTime(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }

    // 协程用于在指定时间后销毁爆炸特效
    IEnumerator DestroyEffectAfterTime(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        if (effect != null)
        {
            Destroy(effect);
        }
    }

    // 更新燃料显示
    void UpdateFuelText()
    {
        if (fuelText != null)
        {
            fuelText.text = "Fuel: " + currentFuel.ToString();
        }
    }

    // 碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        // 检查是否碰撞到标签为 "Fuel" 的物体
        if (other.CompareTag("Fuel"))
        {
            // 增加燃料
            currentFuel += 20;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); // 确保燃料不超过最大值

            // 更新燃料显示
            UpdateFuelText();

            // 播放触碰音效
            if (audioSource != null && touchSoundClip != null)
            {
                audioSource.PlayOneShot(touchSoundClip);
            }

            // 销毁燃料模型
            Destroy(other.gameObject);
        }

        // 检查是否碰撞到标签为 "yanjiang" 的物体
        if (other.CompareTag("yanjiang"))
        {
           fire.SetActive(true);
            isTouchingYanjiang = true; // 开始减少燃料
        }
        // 检查是否碰撞到标签为 "Barrel" 的物体（炸药桶）
        if (other.CompareTag("Barrel"))
        {
            // 减少燃料
            currentFuel -= 10;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel); // 确保燃料不小于0
            UpdateFuelText(); // 更新燃料显示
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检查是否离开标签为 "yanjiang" 的物体
        if (other.CompareTag("yanjiang"))
        {
            fire.SetActive(false);
            isTouchingYanjiang = false; // 停止减少燃料
        }
    }
}
