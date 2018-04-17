using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {
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

    List<CharacterData> characters;

    public int GetTotalMaxHealth()
    {
        int sum = 0;
        for(int i = 0; i < characters.Count; i++)
        {
            sum += characters[i].MaxHp;
        }

        return sum;
    }

    public int GetTotalCurrentHealth()
    {
        int sum = 0;
        for (int i = 0; i < characters.Count; i++)
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
