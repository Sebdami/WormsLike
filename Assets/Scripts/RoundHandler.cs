using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour {
    int numberOfTeams = 2;
    int[] lastActivePlayerForTeam;
    int currentTeamRound = 0;
    [SerializeField]
    float roundMaxTimer = 5.0f;

    float roundTimer = 0.0f;

	// Use this for initialization
	void Start () {
        lastActivePlayerForTeam = new int[numberOfTeams];

    }

	void Update () {
        roundTimer += Time.deltaTime;
        if(roundTimer > roundMaxTimer)
        {
            roundTimer = 0.0f;
        }
	}

    void NextRound()
    {
        currentTeamRound = (currentTeamRound + 1) % numberOfTeams;
    }

    void SwitchPlayer()
    {
        lastActivePlayerForTeam[currentTeamRound] = 1;
    }

}
