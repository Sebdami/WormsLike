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
            CharacterInstance inst = GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<CharacterInstance>();
            inst.GetComponent<WormController>().CurrentState = inst.GetComponent<WormController>().CurrentState == WormState.Dead? WormState.Dead : WormState.Paused;
            inst.characterData.OnHealthChange += OnCurrentCharacterHealthChange;
            currentActivePlayerIndex = value;
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Movement;
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<CharacterInstance>().characterData.OnHealthChange -= OnCurrentCharacterHealthChange;
            //Remove also WormController OnStateChnage
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

    void OnCurrentCharacterHealthChange(int diff)
    {
        if(diff < 0)
        {
            //Use worm controller OnStateChange
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
        for (int i = 1; i < teamCharactersAmount; i++)
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
