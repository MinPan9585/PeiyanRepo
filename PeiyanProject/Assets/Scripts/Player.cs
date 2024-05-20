using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.MovePosition(transform.position + new Vector3(0,0,speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.MovePosition(transform.position + new Vector3(speed * Time.deltaTime,0, 0 ));
        }
    }
}
