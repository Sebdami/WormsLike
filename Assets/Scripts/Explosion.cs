using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Explosion
{
    public float ExplosionRadius = 6.0f;
    public float ExplosionForce = 10.0f;
    public int Damage = 50;
    public AnimationCurve DamageFalloff = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
    public float UpLiftForce = 2.0f;
    public ParticleSystem particleSystem;
}
