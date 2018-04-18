using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInstance : MonoBehaviour {

    public GameObject characterInfo;

    public CharacterData characterData;

    Text hpText;
    Text nameText;

    public int CurrentHp
    {
        get { return characterData.CurrentHp; }
        set
        {
            characterData.CurrentHp = value;
            if(!hpText)
                hpText = characterInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>();

            hpText.text = value.ToString();
        }
    }

    public void InitUI()
    {
        characterInfo.GetComponent<UIFollowCharacter>().character = this;
        nameText = characterInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        hpText = characterInfo.transform.GetChild(1).GetChild(0).GetComponent<Text>();

        nameText.text = characterData.CharacterName;
        nameText.color = GameManager.instance.teams[characterData.Team].teamColor;
        hpText.text = characterData.CurrentHp.ToString();
        hpText.color = GameManager.instance.teams[characterData.Team].teamColor;
    }

    private void OnDestroy()
    {
        Destroy(characterInfo);
    }

}
