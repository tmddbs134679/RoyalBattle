using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCamera : QuantumViewComponent<CameraViewContext>
{
 
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(ViewContext.virtualCamera.transform);
    }
}
