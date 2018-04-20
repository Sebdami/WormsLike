using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour {

    public int WeaponId;
    public int amount;

    public void Init(int _id, int _amount)
    {
        WeaponId = _id;
        amount = _amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterInstance ci = other.GetComponentInParent<CharacterInstance>();
        if (ci)
        {
            GameManager.instance.teams[ci.characterData.Team].AddWeaponAmmo(WeaponId, amount);
            GameManager.instance.inventory.InitFromCurrentTeam();
            Destroy(gameObject);
        }
    }
}
