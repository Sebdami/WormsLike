using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamHealthBar : MonoBehaviour {
    [SerializeField]
    int teamIndex = 0;
    Team team;
    Image healthBar;

	void Start () {
        team = GameManager.instance.teams[teamIndex];
        Text nameText = GetComponentInChildren<Text>();
        nameText.text = team.TeamName;
        nameText.color = team.teamColor;
        healthBar = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        healthBar.color = team.teamColor;
        healthBar.fillAmount = team.GetTotalHealthPercent();
	}
	
	void Update () {
        //This is bad, should clearly be done only when health is lost
        healthBar.fillAmount = team.GetTotalHealthPercent();
    }
}
