using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public delegate void WeaponEvent();
    public WeaponEvent OnWeaponStartUse;
    public WeaponEvent OnWeaponEndUse;

    public WormController ownerController;
    protected void Start()
    {
        ownerController = GetComponentInParent<WormController>();
        ownerController.OnStateChange += OnControllerStateChange;
    }

    protected void OnDestroy()
    {
        ownerController.OnStateChange -= OnControllerStateChange;
    }

    protected virtual void OnControllerStateChange(WormState oldState, WormState newState)
    {

    }
}
