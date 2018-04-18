using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData {
    public delegate void CharacterEvent();
    public delegate void CharacterHealthEvent(int diff);

    public CharacterEvent OnDeath;
    public CharacterHealthEvent OnHealthChange;

    int id;
    [SerializeField]
    string characterName = "Jean René";

    [SerializeField]
    int maxHp = 100;
    int currentHp = 100;

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
            int diff = value - currentHp;
            currentHp = value;
            if (currentHp <= 0)
                IsAlive = false;
            if (currentHp > maxHp)
                maxHp = currentHp;
            if (OnHealthChange != null)
                OnHealthChange(diff);
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
            if (value != isAlive)
            {
                isAlive = value;
                if (!value)
                {
                    if (OnDeath != null)
                    {
                        OnDeath();
                    }
                }
            }
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
}
