using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private float chargeTime;
    private bool isCharging;
    private Vector3 jumpDirection;

    void Start()
    {
        // 游戏开始时禁用喷射烟雾特效
        if (jumpEffect != null)
        {
            jumpEffect.SetActive(false);
        }
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
    }

    void Jump()
    {
        float chargeRatio = chargeTime / maxChargeTime;
        float jumpForce = Mathf.Sqrt(-2f * gravity * maxJumpHeight * chargeRatio);
        float horizontalForce = Mathf.Sqrt(2f * maxJumpDistance * chargeRatio * -gravity);

        Vector3 jumpVector = jumpDirection * horizontalForce + Vector3.up * jumpForce;

        GetComponent<Rigidbody>().AddForce(jumpVector, ForceMode.Impulse);

        isCharging = false;
        chargeTime = 0f;
        energyBarImage.fillAmount = 0f;

        // 播放跳跃音效
        AudioSource audioSource = GetComponent<AudioSource>();
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
}