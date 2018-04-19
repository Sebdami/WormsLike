using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITotalTimer : MonoBehaviour {

    Text timerText;
    private void Start()
    {
        timerText = GetComponent<Text>();
    }
    private void Update()
    {
        timerText.text = ((int)GameManager.instance.gameTimer + 1).ToString();
    }
}
