using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCarRotate : MonoBehaviour{
    public float incr;
    private void Start(){
        incr = 40f;
    }
    void Update(){   
        transform.Rotate(0, incr * Time.deltaTime, 0, Space.Self);
    }
}