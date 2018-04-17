using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsManager : MonoBehaviour {
    public Team[] teams;

    public Team[] GetInitializedTeams()
    {
        int characterId = 0;
        for(int i = 0; i < teams.Length; i++)
        {
            teams[i].Id = i;
            for(int j = 0; j < teams[i].characters.Length; j++)
            {
                teams[i].characters[j].Id = characterId++;
                teams[i].characters[j].Team = i;
                teams[i].characters[j].CurrentHp = teams[i].characters[j].MaxHp;
            }
        }
        return teams;
    }
}
