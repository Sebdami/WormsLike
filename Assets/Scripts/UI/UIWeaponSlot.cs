using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponSlot : MonoBehaviour {
    public WeaponData weaponData;
    UIInventory parentInventory;
    Button slotButton;
    Text textAmount;
    Text textInfinite;

    Button SlotButton
    {
        get
        {
            if (!slotButton)
                slotButton = transform.GetChild(0).GetComponent<Button>();
            return slotButton;
        }

        set
        {
            slotButton = value;
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

    public void UpdateSlot(WeaponData weapon)
    {
        weaponData = weapon;
        if(weaponData == null)
        {
            ResetSlot();
        }
        else
        {
            if(weapon.InfiniteAmmo)
            {
                TextAmount.gameObject.SetActive(false);
                TextInfinite.gameObject.SetActive(true);
            }
            else
            {
                TextAmount.gameObject.SetActive(true);
                TextInfinite.gameObject.SetActive(false);
                TextAmount.text = weapon.CurrentAmmo.ToString();
                if (weapon.CurrentAmmo == 0)
                    SlotButton.interactable = false;
            }
            SlotButton.GetComponent<Image>().sprite = weapon.WeaponImage;
            SlotButton.onClick.RemoveAllListeners();
            SlotButton.onClick.AddListener(() => ParentInventory.WeaponClicked(weaponData));
        }
    }

    public void ResetSlot()
    {
        TextAmount.gameObject.SetActive(false);
        TextInfinite.gameObject.SetActive(false);
        SlotButton.GetComponent<Image>().sprite = GameManager.instance.EmptySlotSprite;
        SlotButton.onClick.RemoveAllListeners();
    }
}
