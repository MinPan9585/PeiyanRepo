using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireFountain : MonoBehaviour
{
    // ָ����Ч������ϵͳ
    public GameObject effect;

    public int Force;

    // ��������
    private void DestroyObject()
    {
        Destroy(gameObject); // ���ٵ�ǰ����
    }



    // ������ʩ�����ϵ���
    private void ApplyForceToPlayer(Rigidbody playerRigidbody)
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.AddForce(Vector3.up * Force, ForceMode.Impulse); // ���Ը�����Ҫ�������Ĵ�С
        }
    }

    // ��ײ���
    private void OnTriggerEnter(Collider other)
    {
        // ������ҵı�ǩ�� "Player"
        if (other.CompareTag("Character"))
        {

            effect.SetActive(true);

            ApplyForceToPlayer(other.GetComponent<Rigidbody>()); // ��ȡ��ҵ� Rigidbody ��ʩ����
            DestroyObject(); // ���ٵ�ǰ����
        }
    }
}