using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalLapCam : MonoBehaviour
{
    public Transform car;
    void Update()
    {
        transform.LookAt(car);
    }
}
