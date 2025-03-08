using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    public GameObject explosionEffectPrefab; // 爆炸特效预制体
    public float explosionForce = 500f;      // 爆炸力大小
    public float explosionRadius = 5f;       // 爆炸范围

    private void OnTriggerEnter(Collider other)
    {
        // 检测是否是角色（假设角色有特定的Tag，如"Player"）
        if (other.CompareTag("Character"))
        {
            Explode();
        }
    }

    private void Explode()
    {


        explosionEffectPrefab.SetActive(true);
        // 播放爆炸特效
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 对周围物体施加爆炸力
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Character")) // 确保只对角色施加力
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        // 销毁爆炸桶
        Destroy(gameObject);
    }
}
