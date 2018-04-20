using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : TimedExplosiveProjectile {

    private new void Start()
    {
        base.Start();
        Destroy(gameObject, 10.0f);
    }

    public override void Launch(Vector3 direction, float force)
    {
        base.Launch(direction, force);
        rb.AddForce(Vector3.up * 2.0f);
        GetComponent<Rigidbody>().AddTorque(Vector3.forward * force * 2.0f);
    }

    public override void Explode()
    {
        base.Explode();
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject, 2.5f);
        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<ConstantForce>());
        Destroy(GetComponent<Rigidbody>());
    }
}
