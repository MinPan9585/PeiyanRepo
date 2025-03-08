using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelload : MonoBehaviour
{

    public string nextlevel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            SceneManager.LoadScene(nextlevel);
        }
    }
}
