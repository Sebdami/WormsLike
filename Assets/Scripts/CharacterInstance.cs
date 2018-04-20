using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInstance : MonoBehaviour {
    public delegate void WeaponChangeEvent(WeaponData weapon);
    public WeaponChangeEvent OnWeaponChanged;

    public GameObject characterInfo;

    public CharacterData characterData;

    WormController controller;

    WeaponSocket weaponSocket;

    private Weapon currentWeapon;

    private WeaponData currentWeaponData = null;


    public int CurrentHp
    {
        get { return characterData.CurrentHp; }
        set
        {
            characterData.CurrentHp = value;
        }
    }

    public WormController Controller
    {
        get
        {
            if (!controller)
                controller = GetComponent<WormController>();
            return controller;
        }

        set
        {
            controller = value;
        }
    }

    public Weapon CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }

        set
        {
            if (currentWeapon != null)
                currentWeapon.OnWeaponEndUse -= UpdateOwnWeaponAfterUse;
            currentWeapon = value;
            if (currentWeapon != null)
                currentWeapon.OnWeaponEndUse += UpdateOwnWeaponAfterUse;
        }
    }

    public WeaponSocket WeaponSocket
    {
        get
        {
            if (!weaponSocket)
                weaponSocket = GetComponentInChildren<WeaponSocket>();
            return weaponSocket;
        }

        set
        {
            weaponSocket = value;
        }
    }

    public WeaponData CurrentWeaponData
    {
        get
        {
            return currentWeaponData;
        }

        set
        {
            if(currentWeaponData == value)
            {
                if (currentWeaponData != null && !currentWeaponData.InfiniteAmmo && currentWeaponData.CurrentAmmo <= 0)
                {
                    CurrentWeaponData = null;
                }
                else
                {
                    if (OnWeaponChanged != null)
                        OnWeaponChanged(currentWeaponData);
                }
                if (value != null && currentWeapon == null)
                {
                    InstantiateWeapon(value);
                }
                return;
            }

            while (WeaponSocket.transform.childCount > 0)
                DestroyImmediate(WeaponSocket.transform.GetChild(0).gameObject);

            currentWeaponData = value;
            if(currentWeaponData != null)
            {
                InstantiateWeapon(currentWeaponData);
            }
            else
            {
                CurrentWeapon = null;
            }
            
            if (OnWeaponChanged != null)
                OnWeaponChanged(currentWeaponData);
        }
    }

    void InstantiateWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return;
        Weapon toApply = Instantiate(weapon.WeaponPrefab, WeaponSocket.transform).GetComponent<Weapon>();
        toApply.transform.localPosition = Vector3.zero;
        toApply.transform.localRotation = Quaternion.identity;
        CurrentWeapon = toApply;
        CurrentWeapon.weaponData = weapon;
    }

    void UpdateOwnWeaponAfterUse()
    {
        CurrentWeaponData = currentWeaponData;
    }

    public void Select()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
        characterInfo.transform.SetAsLastSibling();
        CurrentWeaponData = currentWeaponData;
    }

    public void Deselect()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
        if(CurrentWeapon)
            Destroy(CurrentWeapon.gameObject);
    }

    private void OnDestroy()
    {
        Destroy(characterInfo);
    }

}
