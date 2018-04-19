using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WeaponData {
    int id;

    [SerializeField]
    string weaponName;

    [SerializeField]
    Image weaponImage;

    [SerializeField]
    GameObject weaponPrefab;

    [SerializeField]
    int maxAmmo;

    int currentAmmo;

    [SerializeField]
    bool infiniteAmmo;

    public string WeaponName
    {
        get
        {
            return weaponName;
        }

        set
        {
            weaponName = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public Image WeaponImage
    {
        get
        {
            return weaponImage;
        }

        set
        {
            weaponImage = value;
        }
    }

    public GameObject WeaponPrefab
    {
        get
        {
            return weaponPrefab;
        }

        set
        {
            weaponPrefab = value;
        }
    }

    public int MaxAmmo
    {
        get
        {
            return maxAmmo;
        }

        set
        {
            maxAmmo = value;
        }
    }

    public int CurrentAmmo
    {
        get
        {
            return currentAmmo;
        }

        set
        {
            currentAmmo = value;
        }
    }

    public bool InfiniteAmmo
    {
        get
        {
            return infiniteAmmo;
        }

        set
        {
            infiniteAmmo = value;
        }
    }
}
