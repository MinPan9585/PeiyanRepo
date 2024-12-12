using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MoveBear : MonoBehaviour
{
    Transform bearTrans;
    Vector3 bearPos;
    Vector3 positionOne;
    Vector3 positionTwo;

    Vector3 direction;
    public float speed;
    bool isHeadingToTwo = true;

    private void Start()
    {
        bearTrans = transform.GetChild(0);
        positionOne = transform.GetChild(1).position;
        positionTwo = transform.GetChild(2).position;
    }

    private void Update()
    {
        if (isHeadingToTwo)
        {
            direction = positionTwo - positionOne;
        }
        else
        {
            direction = positionOne - positionTwo;
        }

        if(isHeadingToTwo && Vector3.Distance(bearTrans.position, positionTwo) <= 0.1f)
        {
            isHeadingToTwo=false;
            bearTrans.Rotate(new Vector3(0,180f,0));
        }
        if(!isHeadingToTwo && Vector3.Distance(bearTrans.position, positionOne) <= 0.1f)
        {
            isHeadingToTwo=true;
            bearTrans.Rotate(new Vector3(0, 180f, 0));
        }

        bearTrans.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
}
