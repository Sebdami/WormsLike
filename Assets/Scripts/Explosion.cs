using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Explosion
{
    public float ExplosionRadius = 6.0f;
    public float ExplosionForce = 10.0f;
    public int maxDamage = 50;
    public int minDamage = 5;
    public AnimationCurve DamageFalloff = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
    public float UpLiftForce = 2.0f;
    public ParticleSystem particleSystem;

    public void Explode(Vector3 position)
    {
        GameManager.instance.world.GetComponent<ModifyTerrain>().SphereAtPosition(position, ExplosionRadius, 0);
        Collider[] affectedColliders = Physics.OverlapSphere(position, ExplosionRadius, LayerMask.GetMask("Worm"));
        foreach (Collider col in affectedColliders)
        {
            col.GetComponent<WormController>().CurrentState = WormState.Hit;
            float damageFallOffMultiplier = DamageFalloff.Evaluate(Vector3.Distance(position, col.transform.position) / ExplosionRadius);

            col.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce * damageFallOffMultiplier, position, ExplosionRadius, UpLiftForce * damageFallOffMultiplier, ForceMode.Impulse);
            // Apply damage with falloff
            col.GetComponent<CharacterInstance>().CurrentHp -= (int)(Mathf.Lerp(minDamage, maxDamage,damageFallOffMultiplier));
        }
        GameObject.Destroy(GameObject.Instantiate(particleSystem, position, Quaternion.identity), 3.0f);
    }
}
