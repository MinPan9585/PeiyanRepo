using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireFountain : MonoBehaviour
{
    // 指定特效的粒子系统
    public GameObject effect;

    public int Force;

    // 销毁物体
    private void DestroyObject()
    {
        Destroy(gameObject); // 销毁当前物体
    }



    // 给物体施加向上的力
    private void ApplyForceToPlayer(Rigidbody playerRigidbody)
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.AddForce(Vector3.up * Force, ForceMode.Impulse); // 可以根据需要调整力的大小
        }
    }

    // 碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        // 假设玩家的标签是 "Player"
        if (other.CompareTag("Character"))
        {

            effect.SetActive(true);

            ApplyForceToPlayer(other.GetComponent<Rigidbody>()); // 获取玩家的 Rigidbody 并施加力
            DestroyObject(); // 销毁当前物体
        }
    }
}