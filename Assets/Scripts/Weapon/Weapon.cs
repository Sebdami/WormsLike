using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public delegate void WeaponEvent();
    public WeaponEvent OnWeaponStartUse;
    public WeaponEvent OnWeaponEndUse;

    public int maxUsePerRound = 1;
    public int currentRoundUsesLeft;

    public WormController ownerController;
    protected void Start()
    {
        ownerController = GetComponentInParent<WormController>();
        currentRoundUsesLeft = maxUsePerRound;
    }

    public void ResetUses()
    {
        currentRoundUsesLeft = maxUsePerRound;
    }
}
