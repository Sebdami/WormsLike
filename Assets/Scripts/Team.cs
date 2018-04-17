using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team {
    int id;

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
    [SerializeField]
    public Color teamColor;

    [SerializeField]
    public CharacterData[] characters;
    [HideInInspector]
    public GameObject[] characterInstances;

    public int GetTotalMaxHealth()
    {
        int sum = 0;
        for(int i = 0; i < characters.Length; i++)
        {
            sum += characters[i].MaxHp;
        }

        return sum;
    }

    public int GetTotalCurrentHealth()
    {
        int sum = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            sum += characters[i].CurrentHp;
        }

        return sum;
    }

    public float GetTotalHealthPercent()
    {
        return (float)GetTotalCurrentHealth() / (float)GetTotalMaxHealth();
    }
}
