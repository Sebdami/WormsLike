using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile {
    public float ExplosionRadius = 6.0f;
    public float ExplosionForce = 10.0f;
    public float UpLiftForce = 2.0f;

    public virtual void Explode()
    {
        GameManager.instance.world.GetComponent<ModifyTerrain>().SphereAtPosition(transform.position, ExplosionRadius, 0);
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask.GetMask("Worm"));
        foreach (Collider col in affectedColliders)
        {
            col.GetComponent<WormController>().CurrentState = WormState.Hit;
            col.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, UpLiftForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
}
