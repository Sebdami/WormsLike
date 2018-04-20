using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsDatabase : MonoBehaviour {
    [SerializeField]
    public Team[] teams;
    [SerializeField]
    public int[] selectedTeamsIndices;
    // Return a usable copy from the values set in the inspector
    public Team[] GetInitializedTeams()
    {
        Team[] copy = new Team[teams.Length];
        int characterId = 0;
        for(int i = 0; i < selectedTeamsIndices.Length; i++)
        {
            copy[i] = new Team();
            copy[i].Id = i;
            copy[i].teamColor = teams[selectedTeamsIndices[i]].teamColor;
            copy[i].TeamName = teams[selectedTeamsIndices[i]].TeamName;
            copy[i].characters = new CharacterData[teams[selectedTeamsIndices[i]].characters.Length];
            for (int j = 0; j < teams[selectedTeamsIndices[i]].characters.Length; j++)
            {
                copy[i].characters[j] = new CharacterData();
                copy[i].characters[j].Id = characterId++;
                copy[i].characters[j].Team = i;
                copy[i].characters[j].MaxHp = teams[selectedTeamsIndices[i]].characters[j].MaxHp;
                copy[i].characters[j].CurrentHp = teams[selectedTeamsIndices[i]].characters[j].MaxHp;
                copy[i].characters[j].CharacterName = teams[selectedTeamsIndices[i]].characters[j].CharacterName;
            }
        }
        return copy;
    }
}
