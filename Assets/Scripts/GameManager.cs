using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Team[] teams;

    [SerializeField]
    GameObject levelCanvasPrefab;

    [SerializeField]
    GameObject teamsManagerPrefab;

    [SerializeField]
    GameObject weaponDatabasePrefab;

    [SerializeField]
    GameObject characterPrefab;
    [SerializeField]
    GameObject characterInfoPrefab;

    public float gameMaxTime = 600f;
    [HideInInspector]
    public float gameTimer = 0.0f;

    public World world;

    public RoundHandler roundHandler;

    WeaponDatabase weaponDatabase;
    [HideInInspector]
    public GameObject LevelCanvas;

    Water water;
    SpawnPoints spawnpoints;
    public bool suddenDeath = false;

    public Sprite EmptySlotSprite;

    public UIInventory inventory;

    public WeaponDatabase WeaponDb
    {
        get
        {
            if (!weaponDatabase)
                weaponDatabase = weaponDatabasePrefab.GetComponent<WeaponDatabase>();
            return weaponDatabase;
        }

        set
        {
            weaponDatabase = value;
        }
    }

    void Awake () {
		if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LevelCanvas = Instantiate(levelCanvasPrefab);
        teams = teamsManagerPrefab.GetComponent<TeamsDatabase>().GetInitializedTeams();
        for(int i = 0; i < teams.Length; i++)
        {
            teams[i].TeamWeapons = WeaponDb.GetWeapons();
        }
        world = FindObjectOfType<World>();
        water = FindObjectOfType<Water>();
        spawnpoints = FindObjectOfType<SpawnPoints>();
        roundHandler = FindObjectOfType<RoundHandler>();
        inventory = FindObjectOfType<UIInventory>();
	}

    private void Start()
    {
        gameTimer = gameMaxTime;
        foreach(Team team in teams)
        {
            team.characterInstances = new GameObject[team.characters.Length];
            for(int i = 0; i < team.characters.Length; i++)
            {
                team.characterInstances[i] = Instantiate(characterPrefab);
                CharacterInstance currentInstance = team.characterInstances[i].GetComponent<CharacterInstance>();
                currentInstance.transform.position = spawnpoints.GetRandomSpawnPoint();
                currentInstance.characterData = team.characters[i];
                currentInstance.characterInfo = Instantiate(characterInfoPrefab, LevelCanvas.transform);
                currentInstance.characterInfo.GetComponent<UICharacterInfo>().Init(currentInstance);
                currentInstance.GetComponent<WormController>().CurrentState = WormState.Paused;
                currentInstance.characterData.OnDeath += CheckVictory;
            }
        }
    }
    
    private void Update()
    {
        if(!suddenDeath)
            gameTimer -= Time.deltaTime;

        if(gameTimer <= 0 && !suddenDeath)
        {
            gameTimer = 0.0f;
            suddenDeath = true;
            water.StartRising();
        }
    }

    public void CheckVictory()
    {
        List<Team> aliveTeams = new List<Team>(); 

        for(int i = 0; i < teams.Length; i++)
        {
            if (!teams[i].IsTeamDead())
                aliveTeams.Add(teams[i]);
        }

        if(aliveTeams.Count == 0)
        {
            Team noTeam = new Team();
            noTeam.TeamName = "NO ONE";
            noTeam.teamColor = Color.white;
            WinGame(noTeam);
        }
        else if(aliveTeams.Count == 1)
        {
            WinGame(aliveTeams[0]);
        }
    }

    public CharacterInstance[] GetAliveCharacters()
    {
        List<CharacterInstance> toReturn = new List<CharacterInstance>();
        foreach (Team team in teams)
        {
            foreach(GameObject go in team.characterInstances)
            {
                CharacterInstance toAdd = go.GetComponent<CharacterInstance>();
                if (toAdd && toAdd.characterData.IsAlive)
                    toReturn.Add(toAdd);
            }
        }
        return toReturn.ToArray();
    }

    public void WinGame(Team winningTeam)
    {
        LevelCanvas.GetComponentInChildren<UIRoundAnouncer>().gameObject.SetActive(false);
        UICharacterInfo[] characterInfos = LevelCanvas.GetComponentsInChildren<UICharacterInfo>();
        foreach(UICharacterInfo cha in characterInfos)
        {
            cha.gameObject.SetActive(false);
        }
        Time.timeScale = 0;
        UIWinPanel winPanel = LevelCanvas.GetComponentInChildren<UIWinPanel>(true);
        winPanel.UpdateVisual(winningTeam.TeamName, winningTeam.teamColor);
        winPanel.gameObject.SetActive(true);
        winPanel.transform.SetAsLastSibling();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
