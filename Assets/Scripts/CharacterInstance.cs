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
            currentWeapon = value;
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
                if (OnWeaponChanged != null)
                    OnWeaponChanged(currentWeaponData);
                return;
            }

            while (WeaponSocket.transform.childCount > 0)
                DestroyImmediate(WeaponSocket.transform.GetChild(0).gameObject);

            currentWeaponData = value;
            Weapon toApply = Instantiate(currentWeaponData.WeaponPrefab, WeaponSocket.transform).GetComponent<Weapon>();
            toApply.transform.localPosition = Vector3.zero;
            toApply.transform.localRotation = Quaternion.identity;
            CurrentWeapon = toApply;
            CurrentWeapon.weaponData = currentWeaponData;
            if (OnWeaponChanged != null)
                OnWeaponChanged(currentWeaponData);
        }
    }

    public void Select()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
        characterInfo.transform.SetAsLastSibling();
    }

    public void Deselect()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
    }

    private void OnDestroy()
    {
        Destroy(characterInfo);
    }

}
