using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaProjectile : ExplosiveProjectile {
    [SerializeField]
    float launchForce = 20.0f;
    private new void Start()
    {
        base.Start();
        Launch(transform.forward, launchForce) ;
    }

    public override void Explode()
    {
        base.Explode();
    }
}
