using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class seeThrough : MonoBehaviour
{
    private void Awake()
    {
        PXR_Boundary.EnableSeeThroughManual(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
