using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {
    int id;
    int maxHp = 100;
    int currentHp = 100;
    string characterName = "Jean René";
    int team = 0;
    bool isAlive = true;

    public void Init()
    {

    }

    public void Init(int _id, int _team, string _characterName, int _maxHp = 100)
    {
        id = _id;
        Team = _team;
        CharacterName = _characterName;
        MaxHp = maxHp;
        currentHp = maxHp;
    }

    public void GetHit(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
            IsAlive = false;
    }

    public int MaxHp
    {
        get
        {
            return maxHp;
        }

        set
        {
            maxHp = value;
        }
    }

    public int CurrentHp
    {
        get
        {
            return currentHp;
        }

        set
        {
            currentHp = value;
        }
    }

    public string CharacterName
    {
        get
        {
            return characterName;
        }

        set
        {
            characterName = value;
        }
    }

    public int Team
    {
        get
        {
            return team;
        }

        set
        {
            team = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }
}
