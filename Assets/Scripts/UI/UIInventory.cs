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

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.I))
        {
            TogglePanelVisibility();
        }
    }

    void RoundChanged()
    {
        InitFromTeam(GameManager.instance.teams[Rh.CurrentActiveTeam]);
    }

    public void InitFromTeam(Team team)
    {
        int size = Mathf.Min(team.TeamWeapons.Length, 12);
        for(int i = 0; i < size; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<UIWeaponSlot>().UpdateSlot(team.TeamWeapons[i]);
        }
    }

    public void InitFromCurrentTeam()
    {
        Team team = GameManager.instance.teams[GameManager.instance.roundHandler.CurrentActiveTeam];
        int size = Mathf.Min(team.TeamWeapons.Length, 12);
        for (int i = 0; i < size; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<UIWeaponSlot>().UpdateSlot(team.TeamWeapons[i]);
        }
    }

    public void WeaponClicked(WeaponData weapon)
    {
        if (!Rh.hasUsedWeaponOnce && Rh.CurrentActiveCharacter.Controller.CurrentState != WormState.WeaponHandled && weapon != Rh.CurrentActiveCharacter.CurrentWeaponData)
            Rh.CurrentActiveCharacter.CurrentWeaponData = weapon;
    }

    private void OnDestroy()
    {
        Rh.OnRoundChange -= RoundChanged;
    }

    public void TogglePanelVisibility()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
}
