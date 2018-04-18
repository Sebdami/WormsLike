using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team {
    int id;
    [SerializeField]
    string teamName;
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

    public string TeamName
    {
        get
        {
            return teamName;
        }

        set
        {
            teamName = value;
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
            sum += Mathf.Clamp(characters[i].CurrentHp, 0, characters[i].MaxHp);
        }

        return sum;
    }

    GameObject[] GetAliveCharacters()
    {
        return System.Array.FindAll<GameObject>(characterInstances, x => x.GetComponent<CharacterInstance>().characterData.IsAlive == true);
    }

    public float GetTotalHealthPercent()
    {
        return (float)GetTotalCurrentHealth() / (float)GetTotalMaxHealth();
    }

    public bool IsTeamDead()
    {
        int deadCharacters = 0;
        for (int i = 0; i < characters.Length; i++)
            if (!characters[i].IsAlive)
                deadCharacters++;
        return deadCharacters == characters.Length;
    }
}
