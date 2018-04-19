using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITotalTimer : MonoBehaviour {

    Text timerText;
    bool textChanged = false;

    private void Start()
    {
        timerText = GetComponent<Text>();
        textChanged = false;
    }

    private void Update()
    {
        if(GameManager.instance.suddenDeath)
        {
            if(!textChanged)
            {
                SetSuddenDeathText();
                textChanged = true;
            }
            float h, s, v;
            Color.RGBToHSV(timerText.color, out h, out s, out v);
            h += Time.deltaTime*0.5f;
            if (h > 2 * Mathf.PI)
                h -= 2 * Mathf.PI;
            timerText.color = Color.HSVToRGB(h, s, v);
        }
        else
        {
            timerText.text = TimeFormatUtils.GetFormattedTime(GameManager.instance.gameTimer, TimeFormat.MinSec);
        }
    }

    void SetSuddenDeathText()
    {
        timerText.text = "Sudden Death";
        timerText.color = Color.red;
    }
}
