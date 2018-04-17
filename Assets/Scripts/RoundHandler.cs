using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour {
    int teamAmount;
    int[] lastActivePlayerForTeam;
    int currentActiveTeam = 0;
    int currentActivePlayerIndex = 0;

    [SerializeField]
    float roundMaxTimer = 5.0f;

    float roundTimer = 0.0f;

    public int CurrentActivePlayerIndex
    {
        get
        {
            return currentActivePlayerIndex;
        }

        set
        {
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Paused;
            currentActivePlayerIndex = value;
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Movement;
        }
    }

    public int CurrentActiveTeam
    {
        get
        {
            return currentActiveTeam;
        }

        set
        {
            GameManager.instance.teams[currentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Paused;
            currentActiveTeam = value;
        }
    }

    // Use this for initialization
    void Start () {
        teamAmount = GameManager.instance.teams.Length;
        lastActivePlayerForTeam = new int[teamAmount];
        CurrentActiveTeam = 0;
        CurrentActivePlayerIndex = 0;
    }

	void Update () {
        roundTimer += Time.deltaTime;
        if(roundTimer > roundMaxTimer)
        {
            roundTimer = 0.0f;
            NextRound();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            SwitchPlayer();
        }
	}

    void NextRound()
    {
        CurrentActiveTeam = (CurrentActiveTeam + 1) % teamAmount;
        currentActivePlayerIndex = lastActivePlayerForTeam[CurrentActiveTeam];
        SwitchPlayer();
    }

    void SwitchPlayer()
    {
        int teamCharactersAmount = GameManager.instance.teams[CurrentActiveTeam].characters.Length;
        for (int i = 1; i < teamCharactersAmount - 1; i++)
        {
            if(GameManager.instance.teams[CurrentActiveTeam].characters[(i + CurrentActivePlayerIndex)% teamCharactersAmount].IsAlive)
            {
                CurrentActivePlayerIndex = (i + CurrentActivePlayerIndex) % teamCharactersAmount;
                break;
            }
        }

        lastActivePlayerForTeam[CurrentActiveTeam] = CurrentActivePlayerIndex;
    }

}
