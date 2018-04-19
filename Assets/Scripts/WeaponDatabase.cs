using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour {
    [SerializeField]
    WeaponData[] weapons;


    public WeaponData[] GetWeapons()
    {
        WeaponData[] copy = new WeaponData[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            copy[i] = new WeaponData();
            copy[i].Id = i;
            copy[i].WeaponName = weapons[i].WeaponName;
            copy[i].WeaponImage = weapons[i].WeaponImage;
            copy[i].WeaponPrefab = weapons[i].WeaponPrefab;
            copy[i].StartingAmmo = weapons[i].StartingAmmo;
            copy[i].CurrentAmmo = weapons[i].StartingAmmo;
            copy[i].InfiniteAmmo = weapons[i].InfiniteAmmo;
        }
        return copy;
    }
}
