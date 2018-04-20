using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExplosiveProjectile : Projectile {
    public Explosion explosion;
    public float explodeAfterSeconds = 3.0f;
    float explosionTimer = 0.0f;

    protected new void Start()
    {
        base.Start();
        gameObject.AddComponent<ConstantForce>().force = GameManager.instance.roundHandler.wind * GameManager.instance.roundHandler.windMultiplier/3.0f;
    }

    protected void Update()
    {
        if (!isActive)
            return;

        explosionTimer -= Time.deltaTime;
        if(explosionTimer <= 0.0f)
        {
            Explode();
        }
    }

    public override void Launch(Vector3 direction, float force)
    {
        base.Launch(direction, force);
        explosionTimer = explodeAfterSeconds;
    }

    public virtual void Explode()
    {
        explosion.Explode(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<ConstantForce>())
            Destroy(GetComponent<ConstantForce>());
    }
}
