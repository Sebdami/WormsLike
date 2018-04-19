using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundAnouncer : MonoBehaviour {

    public float anouncementTime = 1.0f;
    public float fadeOutMaxTime = 2.0f;
    public float backTargetAlpha = 0.4f;

    float timer = 0.0f;
    float fadeOutTimer = 0.0f;

    bool isFadingOut = false;
    bool isAnouncing = false;

    Text anouncementText;
    Image back;

    Color backColor;
    Color textColor;
    string anouncementString;
    string characterName;
    Color characterNameColor;

    void Update()
    {
        if(isAnouncing)
        {
            anouncementText.text = anouncementString;
            timer += Time.deltaTime;
            if(timer >= anouncementTime)
            {
                isAnouncing = false;
                isFadingOut = true;
            }
        }
        if(isFadingOut)
        {
            fadeOutTimer -= Time.deltaTime;

            backColor.a = Mathf.Lerp(0.0f, backTargetAlpha, fadeOutTimer / fadeOutMaxTime);
            textColor.a = fadeOutTimer / fadeOutMaxTime;
            characterNameColor.a = textColor.a;
            UpdateAnouncementString();

            back.color = backColor;
            anouncementText.color = textColor;
            anouncementText.text = anouncementString;

            if (fadeOutTimer <= 0)
            {
                fadeOutTimer = 0.0f;
                isFadingOut = false;
                anouncementText.gameObject.SetActive(false);
            }

        }
    }

    void UpdateAnouncementString()
    {
        anouncementString = "It's <color=#" + ColorUtility.ToHtmlStringRGBA(characterNameColor) + ">" + characterName + "</color>'s turn !";
    }

    void FakeUpdateAnouncementString()
    {
        anouncementString = "It's <color=#" + ColorUtility.ToHtmlStringRGBA(backColor) + ">" + characterName + "</color>'s turn !";
    }


    public void Anounce(string _characterName, Color teamColor)
    {
        if(!anouncementText)
        {
            anouncementText = GetComponentInChildren<Text>(true);
        }
        if(!back)
        {
            back = GetComponent<Image>();
        }
        characterName = _characterName;
        anouncementText.gameObject.SetActive(true);
        characterNameColor = teamColor;
        UpdateAnouncementString();
        anouncementText.text = anouncementString;
        isFadingOut = false;
        timer = 0.0f;
        fadeOutTimer = fadeOutMaxTime;
        backColor = back.color;
        textColor = anouncementText.color;

        backColor.a = backTargetAlpha;
        textColor.a = 1.0f;
        back.color = backColor;
        anouncementText.color = textColor;
        isAnouncing = true;
        transform.SetAsLastSibling();

        // Need to use content size fitter manually for it to work properly

        ContentSizeFitter fitter = anouncementText.GetComponent<ContentSizeFitter>();

        fitter.enabled = true;
        fitter.SetLayoutHorizontal();

        fitter = back.GetComponent<ContentSizeFitter>();

        fitter.enabled = true;
        fitter.SetLayoutHorizontal();
        
    }

}
