using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    protected Rigidbody rb;
	protected void Start () {
        rb = GetComponent<Rigidbody>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody>();
	}

    protected void FixedUpdate () {
		if(rb.velocity.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + rb.velocity);
        }
	}

    public void Launch(Vector3 direction, float force)
    {
        direction.Normalize();
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
