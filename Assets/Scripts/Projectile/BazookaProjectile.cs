using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaProjectile : ExplosiveProjectile {
    [SerializeField]
    float launchForce = 20.0f;
    private new void Start()
    {
        base.Start();
    }

    public override void Explode()
    {
        base.Explode();
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject, 2.5f);
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(Instantiate(explosion.particleSystem, transform.position, Quaternion.identity), 2.0f);
    }
}
