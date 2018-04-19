using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrentWeapon : MonoBehaviour {
    public WeaponData weaponData;
    UIInventory parentInventory;
    Image weaponImage;
    Text textAmount;
    Text textInfinite;

    void Start()
    {
        GameManager.instance.roundHandler.OnCurrentPlayerChange += CurrentPlayerChanged;
    }

    Image WeaponImage
    {
        get
        {
            if (!weaponImage)
                weaponImage = transform.GetChild(0).GetComponent<Image>();
            return weaponImage;
        }

        set
        {
            weaponImage = value;
        }
    }

    Text TextAmount
    {
        get
        {
            if (!textAmount)
                textAmount = transform.GetChild(1).GetComponent<Text>();
            return textAmount;
        }

        set
        {
            textAmount = value;
        }
    }

    Text TextInfinite
    {
        get
        {
            if (!textInfinite)
                textInfinite = transform.GetChild(2).GetComponent<Text>();
            return textInfinite;
        }

        set
        {
            textInfinite = value;
        }
    }

    UIInventory ParentInventory
    {
        get
        {
            if (!parentInventory)
                parentInventory = GetComponentInParent<UIInventory>();
            return parentInventory;
        }

        set
        {
            parentInventory = value;
        }
    }

    void CurrentPlayerChanged(CharacterInstance oldCharacter, CharacterInstance newCharacter)
    {
        if(oldCharacter)
            oldCharacter.OnWeaponChanged -= UpdateWeapon;
        newCharacter.OnWeaponChanged += UpdateWeapon;
        UpdateWeapon(newCharacter.CurrentWeaponData);
    }

    public void UpdateWeapon(WeaponData weapon)
    {
        weaponData = weapon;
        if (weaponData == null)
        {
            ResetSlot();
        }
        else
        {
            if (weapon.InfiniteAmmo)
            {
                TextAmount.gameObject.SetActive(false);
                TextInfinite.gameObject.SetActive(true);
            }
            else
            {
                TextAmount.gameObject.SetActive(true);
                TextInfinite.gameObject.SetActive(false);
                TextAmount.text = weapon.CurrentAmmo.ToString();
            }
            WeaponImage.sprite = weapon.WeaponImage;
        }
    }

    public void ResetSlot()
    {
        TextAmount.gameObject.SetActive(false);
        TextInfinite.gameObject.SetActive(false);
        WeaponImage.sprite = GameManager.instance.EmptySlotSprite;
    }
}

