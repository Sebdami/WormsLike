using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsDatabase : MonoBehaviour {
    [SerializeField]
    Team[] teams;
    // Return a usable copy from the values set in the inspector
    public Team[] GetInitializedTeams()
    {
        Team[] copy = new Team[teams.Length];
        int characterId = 0;
        for(int i = 0; i < teams.Length; i++)
        {
            copy[i] = new Team();
            copy[i].Id = i;
            copy[i].teamColor = teams[i].teamColor;
            copy[i].TeamName = teams[i].TeamName;
            copy[i].characters = new CharacterData[teams[i].characters.Length];
            for (int j = 0; j < teams[i].characters.Length; j++)
            {
                copy[i].characters[j] = new CharacterData();
                copy[i].characters[j].Id = characterId++;
                copy[i].characters[j].Team = i;
                copy[i].characters[j].MaxHp = teams[i].characters[j].MaxHp;
                copy[i].characters[j].CurrentHp = teams[i].characters[j].MaxHp;
                copy[i].characters[j].CharacterName = teams[i].characters[j].CharacterName;
            }
        }
        return copy;
    }
}
