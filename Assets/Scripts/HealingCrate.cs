using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCrate : MonoBehaviour {
    [SerializeField]
    int healingValue = 50;

    private void OnTriggerEnter(Collider other)
    {
        CharacterInstance ci = other.GetComponentInParent<CharacterInstance>();
        if (ci)
        {
            ci.CurrentHp += healingValue;
            Destroy(gameObject);
        }
    }
}
