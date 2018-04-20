using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    protected Rigidbody rb;
    public bool isActive = false;
	protected void Start () {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody>();
	}

    public virtual void Launch(Vector3 direction, float force)
    {
        if(!rb)
        {
            rb = GetComponent<Rigidbody>();
            if (!rb)
                rb = gameObject.AddComponent<Rigidbody>();
        }
        direction.Normalize();
        rb.AddForce(direction * force, ForceMode.Impulse);
        isActive = true;
    }
}
