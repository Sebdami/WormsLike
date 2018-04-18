using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGroup : MonoBehaviour {
    public void Play()
    {
        ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            ps.Play();
        }
    }

    public void Stop()
    {
        ParticleSystem[] pss = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            ps.Stop();
        }
    }
}
