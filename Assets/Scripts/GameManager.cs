﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public Team[] teams;

    [SerializeField]
    GameObject levelCanvasPrefab;

    [SerializeField]
    GameObject teamsManagerPrefab;

    [SerializeField]
    GameObject characterPrefab;
    [SerializeField]
    GameObject characterInfoPrefab;

    public World world;

    [HideInInspector]
    public GameObject LevelCanvas;
    
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
        teams = teamsManagerPrefab.GetComponent<TeamsManager>().GetInitializedTeams();
        world = FindObjectOfType<World>();
	}

    private void Start()
    {
        int posIndex = 0;
        foreach(Team team in teams)
        {
            team.characterInstances = new GameObject[team.characters.Length];
            for(int i = 0; i < team.characters.Length; i++)
            {
                team.characterInstances[i] = Instantiate(characterPrefab);
                CharacterInstance currentInstance = team.characterInstances[i].GetComponent<CharacterInstance>();
                currentInstance.transform.position += Vector3.right* posIndex++;
                currentInstance.characterData = team.characters[i];
                currentInstance.characterInfo = Instantiate(characterInfoPrefab, LevelCanvas.transform);
                currentInstance.InitUI();
                currentInstance.GetComponent<WormController>().CurrentState = WormState.Paused;
            }
        }
    }

}
