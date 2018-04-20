using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public delegate void WeaponEvent();
    public WeaponEvent OnWeaponEndUse;

    public int maxUsePerRound = 1;
    [HideInInspector]
    public int currentRoundUsesLeft;
    [HideInInspector]
    public WeaponData weaponData;
    [HideInInspector]
    public WormController ownerController;

    protected void Start()
    {
        ownerController = GetComponentInParent<WormController>();
        currentRoundUsesLeft = maxUsePerRound;
    }

    protected void Update()
    {
        if (ownerController.CurrentState != WormState.Movement && ownerController.CurrentState != WormState.WeaponHandled)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && currentRoundUsesLeft > 0 && (weaponData.InfiniteAmmo || (weaponData.CurrentAmmo > 0 && !weaponData.InfiniteAmmo)))
        {
            OnUseKeyPressed();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnUseKeyReleased();
        }
    }

    protected virtual void OnUseKeyPressed()
    { }

    protected virtual void OnUseKeyReleased()
    { }

    protected void DecreaseAmmo()
    {
        currentRoundUsesLeft--;
        if(!weaponData.InfiniteAmmo)
            weaponData.CurrentAmmo--;
    }

    public void ResetUses()
    {
        currentRoundUsesLeft = maxUsePerRound;
    }
}
