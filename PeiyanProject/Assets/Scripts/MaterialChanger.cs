using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public GameObject[] objectsToChange;

    [Header("材质设置")]
    public Material touchedMaterial; // 触碰后的目标材质
    public float transitionDuration = 0.5f; // 材质过渡时间

    [Header("音效与特效")]
    public AudioClip touchSound; // 触碰音效
    public ParticleSystem touchEffect; // 触碰粒子效果

    private bool isTransitioning = false; // 是否正在过渡

    // 碰撞检测（触发器模式）
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionAllMaterials());
            PlayEffects(other.transform.position);
        }
    }

    // 碰撞检测（非触发器模式）
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character") && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionAllMaterials());
            PlayEffects(collision.contacts[0].point);
        }
    }

    // 为所有物体启动材质过渡
    private System.Collections.IEnumerator TransitionAllMaterials()
    {
        // 存储所有物体的渲染器和原始材质
        List<Renderer> renderers = new List<Renderer>();
        List<Material[]> originalMaterials = new List<Material[]>();

        foreach (GameObject obj in objectsToChange)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderers.Add(renderer);
                originalMaterials.Add(renderer.materials);
            }
        }

        // 统一过渡所有物体的材质
        float elapsedTime = 0;
        while (elapsedTime < transitionDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            for (int i = 0; i < renderers.Count; i++)
            {
                Material[] newMaterials = new Material[renderers[i].materials.Length];
                for (int j = 0; j < newMaterials.Length; j++)
                {
                    Material lerpedMaterial = new Material(originalMaterials[i][j]);
                    lerpedMaterial.Lerp(originalMaterials[i][j], touchedMaterial, t);
                    newMaterials[j] = lerpedMaterial;
                }
                renderers[i].materials = newMaterials;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终状态一致
        foreach (Renderer renderer in renderers)
        {
            Material[] finalMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < finalMaterials.Length; i++)
            {
                finalMaterials[i] = touchedMaterial;
            }
            renderer.materials = finalMaterials;
        }

        isTransitioning = false;
    }

    // 播放触碰效果
    private void PlayEffects(Vector3 position)
    {
        // 播放音效
        if (touchSound != null)
        {
            AudioSource.PlayClipAtPoint(touchSound, position);
        }

        // 生成粒子效果
        if (touchEffect != null)
        {
            Instantiate(touchEffect, position, Quaternion.identity);
        }
    }

   
}