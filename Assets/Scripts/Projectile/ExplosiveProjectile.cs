using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile {
    public Explosion explosion;

    public virtual void Explode()
    {
        GameManager.instance.world.GetComponent<ModifyTerrain>().SphereAtPosition(transform.position, explosion.ExplosionRadius, 0);
        Collider[] affectedColliders = Physics.OverlapSphere(transform.position, explosion.ExplosionRadius, LayerMask.GetMask("Worm"));
        foreach (Collider col in affectedColliders)
        {
            col.GetComponent<WormController>().CurrentState = WormState.Hit;
            float damageFallOffMultiplier = explosion.DamageFalloff.Evaluate(Vector3.Distance(transform.position, col.transform.position) / explosion.ExplosionRadius);

            col.GetComponent<Rigidbody>().AddExplosionForce(explosion.ExplosionForce * damageFallOffMultiplier, transform.position, explosion.ExplosionRadius, explosion.UpLiftForce * damageFallOffMultiplier, ForceMode.Impulse);
            // Apply damage with falloff
            col.GetComponent<CharacterInstance>().CurrentHp -= (int)(explosion.Damage * damageFallOffMultiplier);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        Explode();
    }
}
