using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : MonoBehaviour {
    public bool isPaused = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Unpause();
            else
                Pause();
        }
    }

    public void Restart()
    {
        GameManager.instance.Restart();
    }

    public void Quit()
    {
        GameManager.instance.BackToMainMenu();
    }

	public void Pause()
    {
        if (GameManager.instance.isFinished)
            return;
        Time.timeScale = 0.0f;
        isPaused = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public void Unpause()
    {
        if (GameManager.instance.isFinished)
        {
            isPaused = false;
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        Time.timeScale = 1.0f;
        isPaused = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
