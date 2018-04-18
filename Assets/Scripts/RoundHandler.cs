using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour {
    int teamAmount;
    int[] lastActivePlayerForTeam;
    int currentActiveTeam = 0;
    int currentActivePlayerIndex = 0;
    CharacterInstance currentActiveCharacter = null;


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
            WormController currentController = GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>();
            currentController.CurrentState = currentController.CurrentState == WormState.Dead ? WormState.Dead : WormState.Paused;
            currentController.OnStateChange -= OnStateChange;
            if(currentActiveCharacter)
                currentActiveCharacter.Deselect();
            currentActivePlayerIndex = value;
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Movement;

            currentController = GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>();
            currentController.OnStateChange += OnStateChange;
            currentController.GetComponent<CharacterInstance>().Select();
            currentActiveCharacter = currentController.GetComponent<CharacterInstance>();
        }
    }

    void OnStateChange(WormState oldState, WormState newState)
    {
        if(oldState == WormState.WeaponHandled)
        {
            roundTimer = 5.0f;
        }
        if(oldState == WormState.Hit)
        {
            roundTimer = 0.0f;
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

    bool IsEveryoneReady()
    {
        CharacterInstance[] characters = GameManager.instance.GetAliveCharacters();
        bool ready = true;
        for(int i = 0; i < characters.Length; i++)
        {
            if (characters[i].Controller.CurrentState == WormState.Hit || characters[i].Controller.CurrentState == WormState.WeaponHandled)
                ready = false;
        }

        return ready;
    }

    // Use this for initialization
    void Start () {
        teamAmount = GameManager.instance.teams.Length;
        lastActivePlayerForTeam = new int[teamAmount];
        CurrentActiveTeam = 0;
        CurrentActivePlayerIndex = 0;
        roundTimer = roundMaxTimer;
        //currentActiveCharacter.Select();
    }

	void Update () {
        roundTimer -= Time.deltaTime;
        if(roundTimer <= 0.0f && IsEveryoneReady())
        {
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
        roundTimer = roundMaxTimer;
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
