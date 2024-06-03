using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    Vector3 fireDir;
    Vector3 finalFireDir;
    Vector3 hitPos;
    public Slider slider;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {

            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    hitPos = hit.point;
                }

                fireDir = transform.position - hitPos;
                finalFireDir = new Vector3(fireDir.x, slider.value, fireDir.z);

                rb.AddForce(force * finalFireDir, ForceMode.Impulse);
            }
        }
    }
}
