using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableWeapon : Weapon {

    [SerializeField]
    GameObject DropPrefab;

    new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
        if (ownerController.CurrentState == WormState.WeaponHandled)
        {
            ownerController.CurrentState = WormState.Movement;
        }
    }

    protected override void OnUseKeyPressed()
    {
        base.OnUseKeyPressed();
        Drop();
    }

    void Drop()
    {
        GameObject drop = Instantiate(DropPrefab, transform.position, transform.rotation);
        drop.GetComponent<Droppable>().Drop();

        ownerController.CurrentState = WormState.WeaponHandled;
        DecreaseAmmo();
        if (OnWeaponEndUse != null)
            OnWeaponEndUse();
    }

    
}
