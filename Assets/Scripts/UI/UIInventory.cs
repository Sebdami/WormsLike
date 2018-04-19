using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour {
    RoundHandler rh;

    RoundHandler Rh
    {
        get
        {
            if (!rh)
                rh = GameManager.instance.roundHandler;
            return rh;
        }

        set
        {
            rh = value;
        }
    }

    private void Start()
    {
        Rh.OnRoundChange += RoundChanged;
        RoundChanged();
    }

    void RoundChanged()
    {
        InitFromTeam(GameManager.instance.teams[Rh.CurrentActiveTeam]);
    }

    void InitFromTeam(Team team)
    {
        int size = Mathf.Min(team.TeamWeapons.Length, 12);
        for(int i = 0; i < size; i++)
        {
            transform.GetChild(i).GetComponent<UIWeaponSlot>().UpdateSlot(team.TeamWeapons[i]);
        }
    }

    public void WeaponClicked(WeaponData weapon)
    {
        if (!Rh.hasUsedWeaponOnce && Rh.CurrentActiveCharacter.Controller.CurrentState != WormState.WeaponHandled)
            Rh.CurrentActiveCharacter.CurrentWeaponData = weapon;
    }

    private void OnDestroy()
    {
        Rh.OnRoundChange -= RoundChanged;
    }
}
