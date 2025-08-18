using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public GameObject[] objectsToChange;

    [Header("��������")]
    public Material touchedMaterial; // �������Ŀ�����
    public float transitionDuration = 0.5f; // ���ʹ���ʱ��

    [Header("��Ч����Ч")]
    public AudioClip touchSound; // ������Ч
    public ParticleSystem touchEffect; // ��������Ч��

    private bool isTransitioning = false; // �Ƿ����ڹ���

    // ��ײ��⣨������ģʽ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character") && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionAllMaterials());
            PlayEffects(other.transform.position);
        }
    }

    // ��ײ��⣨�Ǵ�����ģʽ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Character") && !isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionAllMaterials());
            PlayEffects(collision.contacts[0].point);
        }
    }

    // Ϊ���������������ʹ���
    private System.Collections.IEnumerator TransitionAllMaterials()
    {
        // �洢�����������Ⱦ����ԭʼ����
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

        // ͳһ������������Ĳ���
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

        // ȷ������״̬һ��
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

    // ���Ŵ���Ч��
    private void PlayEffects(Vector3 position)
    {
        // ������Ч
        if (touchSound != null)
        {
            AudioSource.PlayClipAtPoint(touchSound, position);
        }

        // ��������Ч��
        if (touchEffect != null)
        {
            Instantiate(touchEffect, position, Quaternion.identity);
        }
    }

   
}