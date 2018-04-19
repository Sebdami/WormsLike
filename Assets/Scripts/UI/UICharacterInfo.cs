using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfo : MonoBehaviour {
    [SerializeField]
    public CharacterInstance character;

    Text nameText;
    Text hpText;

    public void Init(CharacterInstance _character)
    {
        character = _character;

        GetComponent<UIFollowCharacter>().character = character;
        nameText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        hpText = transform.GetChild(1).GetChild(0).GetComponent<Text>();

        nameText.text = character.characterData.CharacterName;
        nameText.color = GameManager.instance.teams[character.characterData.Team].teamColor;
        hpText.text = character.characterData.CurrentHp.ToString();
        hpText.color = GameManager.instance.teams[character.characterData.Team].teamColor;
        character.characterData.OnHealthChange += UpdateHp;
        transform.GetChild(2).GetChild(1).GetComponent<Image>().color = nameText.color;
    }

    private void UpdateHp(int diff)
    {
        hpText.text = character.characterData.CurrentHp.ToString();
    }

    private void OnDestroy()
    {
        if(character)
        {
            character.characterData.OnHealthChange -= UpdateHp;
        }
    }

    public void ToggleActiveMarker()
    {
        transform.GetChild(2).gameObject.SetActive(!transform.GetChild(2).gameObject.activeSelf);
    }
}
