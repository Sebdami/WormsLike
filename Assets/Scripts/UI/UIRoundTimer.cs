using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundTimer : MonoBehaviour {
    RoundHandler roundHandler;
    Text timerText;
	void Start () {
        roundHandler = GameManager.instance.roundHandler;
        timerText = GetComponent<Text>();
    }
	
	void Update () {
        timerText.text = ((int)roundHandler.RoundTimer+1).ToString();
	}
}
