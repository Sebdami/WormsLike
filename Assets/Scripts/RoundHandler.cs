using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour {
    public delegate void RoundEvent();
    public RoundEvent OnRoundChange;
    int teamAmount;
    int[] lastActivePlayerForTeam;
    int currentActiveTeam = 0;
    int currentActivePlayerIndex = 0;
    CharacterInstance currentActiveCharacter = null;
    public Vector3 wind;
    public float windMultiplier = 3.0f;

    [SerializeField]
    float roundMaxTimer = 5.0f;

    float roundTimer = 0.0f;

    bool hasUsedWeaponOnce = false;
    bool hasBeenHit = false;
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
        // Disable weapon if current character gets hit
        if(newState == WormState.Hit)
        {
            currentActiveCharacter.Controller.CurrentWeapon.currentRoundUsesLeft = 0;
            hasBeenHit = true;
        }
        if(newState == WormState.Dead)
        {
            roundTimer = 0.0f;
        }

        if(newState == WormState.WeaponHandled)
        {
            hasUsedWeaponOnce = true;
        }

        if(oldState == WormState.WeaponHandled && currentActiveCharacter.Controller.CurrentWeapon.currentRoundUsesLeft <= 0)
        {
            roundTimer = 5.0f;
        }
        if(oldState == WormState.Hit)
        {
            // Round finished, prevent movements while waiting for everyone to settle
            roundTimer = 0.0f;
            currentActiveCharacter.Controller.CurrentState = WormState.Paused;
        }
    }

    void ChangeWind()
    {
        wind = new Vector3(Random.Range(-3, 4), 0.0f, 0.0f);
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

    public float RoundTimer
    {
        get
        {
            return roundTimer;
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
        ChangeWind();
        teamAmount = GameManager.instance.teams.Length;
        lastActivePlayerForTeam = new int[teamAmount];
        CurrentActiveTeam = 0;
        CurrentActivePlayerIndex = 0;
        roundTimer = roundMaxTimer;
        if (OnRoundChange != null)
            OnRoundChange();
        //currentActiveCharacter.Select();
    }

	void Update () {
        if(roundTimer > 0.0f)
        roundTimer -= Time.deltaTime;

        if (!hasUsedWeaponOnce && !hasBeenHit && currentActiveCharacter.Controller.CurrentState == WormState.Paused)
            currentActiveCharacter.Controller.CurrentState = WormState.Movement;

        if (roundTimer <= 0.0f && IsEveryoneReady())
        {
            NextRound();
        }

        if(Input.GetKeyDown(KeyCode.N) && !hasUsedWeaponOnce && !hasBeenHit) //Prevent from switching when weapon has been used or when hit
        {
            SwitchPlayer();
        }
	}

    void NextRound()
    {
        hasUsedWeaponOnce = false;
        hasBeenHit = false;
        CurrentActiveTeam = (CurrentActiveTeam + 1) % teamAmount;
        currentActivePlayerIndex = lastActivePlayerForTeam[CurrentActiveTeam];
        //Reset Weapon use for everyone
        currentActiveCharacter.Controller.CurrentWeapon.ResetUses();
        SwitchPlayer();
        roundTimer = roundMaxTimer;
        ChangeWind();
        if (OnRoundChange != null)
            OnRoundChange();
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
