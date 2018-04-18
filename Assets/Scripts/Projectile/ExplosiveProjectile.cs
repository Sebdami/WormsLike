using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile {
    public float ExplosionRadius = 6.0f;
    public float ExplosionForce = 10.0f;
    public int Damage = 50;
    public AnimationCurve DamageFalloff = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
    public float UpLiftForce = 2.0f;

    public virtual void Explode()
    {
        GameManager.instance.world.GetComponent<ModifyTerrain>().SphereAtPosition(transform.position, ExplosionRadius, 0);
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask.GetMask("Worm"));
        foreach (Collider col in affectedColliders)
        {
            col.GetComponent<WormController>().CurrentState = WormState.Hit;
            float damageFallOffMultiplier =DamageFalloff.Evaluate(Vector3.Distance(transform.position, col.transform.position) / ExplosionRadius);

            col.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce * damageFallOffMultiplier, transform.position, ExplosionRadius, UpLiftForce * damageFallOffMultiplier, ForceMode.Impulse);
            // Apply damage with falloff
            col.GetComponent<CharacterInstance>().CurrentHp -= (int)(Damage * damageFallOffMultiplier);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
}
