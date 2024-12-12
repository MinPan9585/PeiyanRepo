using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalisland : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject xianshi;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            xianshi.SetActive(true);
        }
    }
}

