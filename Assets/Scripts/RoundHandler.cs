using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour {
    public delegate void RoundEvent();
    public delegate void PlayerChangeEvent(CharacterInstance oldCharacter, CharacterInstance newCharacter);
    public RoundEvent OnRoundChange;
    public PlayerChangeEvent OnCurrentPlayerChange;
    int teamAmount;
    int[] lastActivePlayerForTeam;
    int currentActiveTeam = 0;
    int currentActivePlayerIndex = 0;
    CharacterInstance currentActiveCharacter = null;
    public Vector3 wind;
    public float windMultiplier = 2.0f;

    UIRoundAnouncer anouncer;

    [SerializeField]
    float roundMaxTimer = 5.0f;

    float roundTimer = 0.0f;

    public bool hasUsedWeaponOnce = false;
    public bool hasBeenHit = false;
    public int CurrentActivePlayerIndex
    {
        get
        {
            return currentActivePlayerIndex;
        }

        set
        {
            CharacterInstance oldCharacterInstance = CurrentActiveCharacter;
            WormController currentController = GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>();
            currentController.CurrentState = currentController.CurrentState == WormState.Dead ? WormState.Dead : WormState.Paused;
            currentController.OnStateChange -= OnStateChange;
            if(CurrentActiveCharacter)
                CurrentActiveCharacter.Deselect();
            currentActivePlayerIndex = value;
            GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>().CurrentState = WormState.Movement;

            currentController = GameManager.instance.teams[CurrentActiveTeam].characterInstances[currentActivePlayerIndex].GetComponent<WormController>();
            currentController.OnStateChange += OnStateChange;
            currentController.GetComponent<CharacterInstance>().Select();
            CurrentActiveCharacter = currentController.GetComponent<CharacterInstance>();
            anouncer.Anounce(CurrentActiveCharacter.characterData.CharacterName, GameManager.instance.teams[CurrentActiveTeam].teamColor);
            if(OnCurrentPlayerChange != null)
            {
                OnCurrentPlayerChange(oldCharacterInstance, CurrentActiveCharacter);
            }
        }
    }

    void OnStateChange(WormState oldState, WormState newState)
    {
        // Disable weapon if current character gets hit
        if(newState == WormState.Hit)
        {
            if(CurrentActiveCharacter.CurrentWeapon)
                CurrentActiveCharacter.CurrentWeapon.currentRoundUsesLeft = 0;
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

        if(oldState == WormState.WeaponHandled && ((CurrentActiveCharacter.CurrentWeapon && CurrentActiveCharacter.CurrentWeapon.currentRoundUsesLeft <= 0)||(!CurrentActiveCharacter.CurrentWeapon)))
        {
            roundTimer = 5.0f;
        }
        if(oldState == WormState.Hit)
        {
            // Round finished, prevent movements while waiting for everyone to settle
            roundTimer = 0.0f;
            CurrentActiveCharacter.Controller.CurrentState = WormState.Paused;
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

    public CharacterInstance CurrentActiveCharacter
    {
        get
        {
            return currentActiveCharacter;
        }

        set
        {
            currentActiveCharacter = value;
            if (currentActiveCharacter.CurrentWeapon != null)
                currentActiveCharacter.CurrentWeaponData = currentActiveCharacter.CurrentWeaponData; // To unequip used up weapons
        }
    }

    bool IsEveryoneReady()
    {
        CharacterInstance[] characters = GameManager.instance.GetAliveCharacters();
        bool ready = true;
        for(int i = 0; i < characters.Length; i++)
        {
            if (characters[i].Controller.CurrentState == WormState.Hit || characters[i].Controller.CurrentState == WormState.WeaponHandled || (characters[i].Controller.CurrentState != WormState.Dead && !characters[i].Controller.IsGrounded))
                ready = false;
        }

        return ready;
    }

    // Use this for initialization
    void Start () {
        ChangeWind();
        anouncer = GameManager.instance.LevelCanvas.GetComponentInChildren<UIRoundAnouncer>(true);
        teamAmount = GameManager.instance.teams.Length;
        lastActivePlayerForTeam = new int[teamAmount];
        CurrentActiveTeam = 0;
        CurrentActivePlayerIndex = 0;
        roundTimer = roundMaxTimer;
        if (OnRoundChange != null)
            OnRoundChange();
        //currentActiveCharacter.Select();
    }
    //Force round change after 8 secs of waiting
    float maxWaitTime = 8.0f;
    float waitTimer = 0.0f;
    bool isWaiting = false;
	void Update () {
        if(roundTimer > 0.0f)
        roundTimer -= Time.deltaTime;

        if (!hasUsedWeaponOnce && !hasBeenHit && CurrentActiveCharacter.Controller.CurrentState == WormState.Paused)
            CurrentActiveCharacter.Controller.CurrentState = WormState.Movement;

        if(isWaiting)
        {
            waitTimer += Time.deltaTime;
        }
        if (roundTimer <= 0.0f)
        {
            if(!isWaiting)
            {
                isWaiting = true;
                waitTimer = 0.0f;
            }
            if (CurrentActiveCharacter.Controller.CurrentState != WormState.Hit && CurrentActiveCharacter.Controller.CurrentState != WormState.Dead && CurrentActiveCharacter.Controller.IsGrounded)
                CurrentActiveCharacter.Controller.CurrentState = WormState.Paused;
            if (IsEveryoneReady() || waitTimer > maxWaitTime)
            {
                isWaiting = false;
                waitTimer = 0.0f;
                NextRound();
            }
        }
        else if (isWaiting)
            isWaiting = false;
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
        //Reset Weapon use
        if (CurrentActiveCharacter.CurrentWeapon)
        {
            CurrentActiveCharacter.CurrentWeapon.ResetUses();
        }
        SwitchPlayer();
        roundTimer = roundMaxTimer;
        ChangeWind();
        if (OnRoundChange != null)
            OnRoundChange();
    }

    void SwitchPlayer()
    {
        int teamCharactersAmount = GameManager.instance.teams[CurrentActiveTeam].characters.Length;
        bool found = false;
        for (int i = 1; i < teamCharactersAmount; i++)
        {
            if(GameManager.instance.teams[CurrentActiveTeam].characters[(i + CurrentActivePlayerIndex)% teamCharactersAmount].IsAlive)
            {
                CurrentActivePlayerIndex = (i + CurrentActivePlayerIndex) % teamCharactersAmount;
                found = true;
                break;
            }
            if(i == teamCharactersAmount-1 && !found)
            {
                CurrentActivePlayerIndex = currentActivePlayerIndex;
            }
        }

        lastActivePlayerForTeam[CurrentActiveTeam] = CurrentActivePlayerIndex;
    }

}
