using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class VRSphereControl : MonoBehaviour
{

    [Header("输入 & 参数")]
    public InputActionProperty triggerAction;
    public float maxJumpHeight = 5f;
    public float maxJumpDistance = 10f;
    public float maxChargeTime = 2f;
    public Transform rightHand;
    public Transform chara;

    [Header("UI & 特效")]
    public Image energyBarImage;
    public LineRenderer worldTrajectoryLine;   // ✅ 世界空间的 LineRenderer
    public int trajectoryPoints = 20;
    public float gravity = -9.81f;
    public float trajectoryLineWidth = 0.1f;
    public GameObject jumpEffect;
    public GameObject explosionEffectPrefab;
    public float effectDuration = 1f;
    public float explosionDuration = 1f;
    public GameObject fire;

    [Header("辅助线绘制缩放（不影响真实跳跃）")]
    public float lineDistanceScale = 1f;   // 远度缩放
    public float lineHeightScale = 1f;   // 高度缩放

    [Header("燃料")]
    public int maxFuel = 20;
    public int currentFuel;
    public TMP_Text fuelText;
    public AudioClip touchSoundClip;
    private AudioSource audioSource;

    private float chargeTime;
    private bool isCharging;
    private Vector3 jumpDirection;
    private bool isTouchingYanjiang = false;
    private float yanjiangTimer;

    /*---------- 生命周期 ----------*/

    private void Start()
    {
        currentFuel = maxFuel;
        UpdateFuelText();

        if (jumpEffect) jumpEffect.SetActive(false);
        if (fire) fire.SetActive(false);

        // 保底给音频源
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
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

            DrawTrajectory();          // ✅ 画世界空间辅助线

            if (triggerValue <= 0.1f)
            {
                Jump();
            }
        }
        else
        {
            worldTrajectoryLine.enabled = false;
        }

        // 岩浆减燃料
        if (isTouchingYanjiang)
        {
            yanjiangTimer += Time.deltaTime;
            if (yanjiangTimer >= 2f)
            {
                yanjiangTimer = 0f;
                currentFuel--;
                currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
                UpdateFuelText();
            }
        }
    }

    /*---------- 跳跃 ----------*/

    private void Jump()
    {
        if (currentFuel <= 0) return;

        float chargeRatio = chargeTime / maxChargeTime;
        float jumpForce = Mathf.Sqrt(-2f * gravity * maxJumpHeight * chargeRatio);
        float horizontalForce = Mathf.Sqrt(2f * maxJumpDistance * chargeRatio * -gravity);

        Vector3 jumpDir = rightHand.forward;
        jumpDir.y = 0f;
        jumpDir.Normalize();

        Vector3 jumpVector = jumpDir * horizontalForce + Vector3.up * jumpForce;
        GetComponent<Rigidbody>().AddForce(jumpVector, ForceMode.Impulse);

        isCharging = false;
        chargeTime = 0f;
        energyBarImage.fillAmount = 0f;

        currentFuel--;
        UpdateFuelText();

        /*==== 喷气特效：只跟随，不改朝向 ====*/
        if (jumpEffect)
        {
            // 1. 确保特效跟着角色（位置+旋转）
            jumpEffect.transform.SetParent(chara, false);   // 成为子物体，本地Pose保持
            jumpEffect.transform.position = chara.position; // 强制对齐当前脚底
            jumpEffect.transform.rotation = chara.rotation; // 仅同步角色朝向

            // 2. 开喷
            jumpEffect.SetActive(true);
            StartCoroutine(DisableEffectAfterTime(jumpEffect, effectDuration));
        }
        if (explosionEffectPrefab)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, explosionDuration);
        }
    }

    /*---------- 画轨迹 ----------*/

    private void DrawTrajectory()
    {
        // 1. 启用世界空间绘制（关键修正）
        worldTrajectoryLine.useWorldSpace = true;
        worldTrajectoryLine.enabled = true;
        worldTrajectoryLine.startWidth = trajectoryLineWidth;
        worldTrajectoryLine.endWidth = trajectoryLineWidth;

        // 2. 起点：角色位置（世界空间）
        Vector3 startWorld = chara.position;

        // 3. 计算跳跃方向（水平方向）
        Vector3 dirWorld = rightHand.forward;
        dirWorld.y = 0f;
        dirWorld.Normalize();
        if (dirWorld.sqrMagnitude < 0.1f) // 避免方向为零
        {
            dirWorld = chara.forward;
            dirWorld.y = 0f;
            dirWorld.Normalize();
        }

        // 4. 计算初速度（与 Jump 方法保持一致逻辑，带缩放）
        float chargeRatio = chargeTime / maxChargeTime;
        float jumpForce = Mathf.Sqrt(-2f * gravity * maxJumpHeight * chargeRatio); // 垂直速度
        float horizontalForce = Mathf.Sqrt(2f * maxJumpDistance * chargeRatio * -gravity); // 水平速度
        Vector3 initialVelWorld = dirWorld * horizontalForce + Vector3.up * jumpForce;

        // 应用辅助线缩放（只影响显示，不影响实际跳跃）
        initialVelWorld.x *= lineDistanceScale;
        initialVelWorld.z *= lineDistanceScale;
        initialVelWorld.y *= lineHeightScale;

        // 5. 计算飞行总时间（轨迹落地时间）
        float totalTime = 0;
        if (Physics.gravity.y != 0)
        {
            // 解二次方程：0 = startY + vY*t + 0.5*g*t²
            float discriminant = initialVelWorld.y * initialVelWorld.y - 2 * Physics.gravity.y * startWorld.y;
            if (discriminant >= 0)
            {
                float t1 = (-initialVelWorld.y + Mathf.Sqrt(discriminant)) / Physics.gravity.y;
                float t2 = (-initialVelWorld.y - Mathf.Sqrt(discriminant)) / Physics.gravity.y;
                totalTime = Mathf.Max(t1, t2); // 取落地时间（正数）
            }
        }
        if (totalTime <= 0) totalTime = 2f; // 保底时间，避免无轨迹

        // 6. 生成轨迹点（世界空间直接赋值）
        worldTrajectoryLine.positionCount = trajectoryPoints;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = (float)i / (trajectoryPoints - 1) * totalTime;
            // 计算世界空间位置（运动学公式）
            Vector3 worldPos = startWorld
                             + initialVelWorld * t
                             + 0.5f * Physics.gravity * t * t;
            worldTrajectoryLine.SetPosition(i, worldPos);

            // Debug线（验证轨迹）
            if (i > 0)
            {
                Vector3 prevWorld = startWorld
                                 + initialVelWorld * ((float)(i - 1) / (trajectoryPoints - 1) * totalTime)
                                 + 0.5f * Physics.gravity * Mathf.Pow((float)(i - 1) / (trajectoryPoints - 1) * totalTime, 2);
                Debug.DrawLine(prevWorld, worldPos, Color.red, 0.02f);
            }
        }
    }
    /*---------- 燃料 & 碰撞 ----------*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            currentFuel += 20;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            UpdateFuelText();
            if (audioSource && touchSoundClip) audioSource.PlayOneShot(touchSoundClip);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("yanjiang"))
        {
            fire.SetActive(true);
            isTouchingYanjiang = true;
            yanjiangTimer = 0f;
        }
        if (other.CompareTag("Barrel"))
        {
            currentFuel -= 10;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
            UpdateFuelText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("yanjiang"))
        {
            fire.SetActive(false);
            isTouchingYanjiang = false;
        }
    }

    /*---------- UI & 工具 ----------*/

    private void UpdateFuelText()
    {
        if (fuelText) fuelText.text = "Fuel: " + currentFuel.ToString();
    }

    private IEnumerator DisableEffectAfterTime(GameObject effect, float time)
    {
        yield return new WaitForSeconds(time);
        if (effect) effect.SetActive(false);
    }
}