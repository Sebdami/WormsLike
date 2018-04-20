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
        ContentSizeFitter fitter = teamText.GetComponent<ContentSizeFitter>();
        fitter.enabled = true;
        fitter.SetLayoutHorizontal();

        fitter = transform.GetChild(1).GetChild(1).GetComponent<ContentSizeFitter>();
        fitter.enabled = true;
        fitter.SetLayoutHorizontal();
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
