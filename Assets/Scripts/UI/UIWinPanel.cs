using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinPanel : MonoBehaviour {
    Text teamText;

    public void UpdateVisual(string teamName, Color teamColor)
    {
        if(!teamText)
            teamText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        teamText.text = teamName;
        teamText.color = teamColor;
    }

    public void Restart()
    {
        GameManager.instance.Restart();
    }

    public void Quit()
    {
        GameManager.instance.BackToMainMenu();
    }
}
