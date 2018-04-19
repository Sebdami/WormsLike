using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile {
    public Explosion explosion;

    protected new void Start()
    {
        base.Start();
        gameObject.AddComponent<ConstantForce>().force = GameManager.instance.roundHandler.wind * GameManager.instance.roundHandler.windMultiplier;
    }

    public virtual void Explode()
    {
        explosion.Explode(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive)
            Explode();
    }
}
