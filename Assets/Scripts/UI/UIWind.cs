using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWind : MonoBehaviour {

    RoundHandler roundHandler;
    Text windText;

	void Start () {
        roundHandler = GameManager.instance.roundHandler;
        roundHandler.OnRoundChange += UpdateWindText;
        windText = GetComponent<Text>();
        UpdateWindText();

	}
    private void OnDestroy()
    {
        roundHandler.OnRoundChange -= UpdateWindText;
    }
    void UpdateWindText()
    {
        windText.text = "";
        if (roundHandler.wind.x == 0.0f)
            windText.text = "-";
        else
        {
            for(int i = 0; i < Mathf.Abs(roundHandler.wind.x); i++)
            {
                windText.text += roundHandler.wind.x > 0 ? ">" : "<";
            }
        }
    }
}
