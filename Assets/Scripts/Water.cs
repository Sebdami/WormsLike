using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public float riseSpeed = 0.1f;
    bool isRising = false;

    private void Start()
    {
        isRising = false;
    }

    public void StartRising()
    {
        isRising = true;
    }

    private void Update()
    {
        if(isRising)
        transform.position += Vector3.up * Time.deltaTime * riseSpeed;
    }
}
