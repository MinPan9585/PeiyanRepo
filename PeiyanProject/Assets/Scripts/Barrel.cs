using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    public GameObject explosionEffectPrefab; // ��ը��ЧԤ����
    public float explosionForce = 500f;      // ��ը����С
    public float explosionRadius = 5f;       // ��ը��Χ

    private void OnTriggerEnter(Collider other)
    {
        // ����Ƿ��ǽ�ɫ�������ɫ���ض���Tag����"Player"��
        if (other.CompareTag("Character"))
        {
            Explode();
        }
    }

    private void Explode()
    {


        explosionEffectPrefab.SetActive(true);
        // ���ű�ը��Ч
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // ����Χ����ʩ�ӱ�ը��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Character")) // ȷ��ֻ�Խ�ɫʩ����
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        // ���ٱ�ըͰ
        Destroy(gameObject);
    }
}
