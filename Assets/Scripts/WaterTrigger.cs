using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour {
    [SerializeField]
    GameObject SplashPrefab;
    private void OnTriggerEnter(Collider other)
    {
        CharacterInstance charInst = other.GetComponentInParent<CharacterInstance>();
        if (charInst)
        {
            charInst.CurrentHp = 0;
            charInst.gameObject.SetActive(false);
        }
        else
            Destroy(other.gameObject);
        ParticleGroup pg = Instantiate(SplashPrefab, GetComponent<BoxCollider>().ClosestPoint(other.transform.position), Quaternion.identity).GetComponent<ParticleGroup>();
        pg.Play();
        Destroy(pg, 3.0f);
    }
}
