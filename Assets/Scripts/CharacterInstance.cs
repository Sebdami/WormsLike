using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInstance : MonoBehaviour {

    public GameObject characterInfo;

    public CharacterData characterData;

    WormController controller;

    public int CurrentHp
    {
        get { return characterData.CurrentHp; }
        set
        {
            characterData.CurrentHp = value;
        }
    }

    public WormController Controller
    {
        get
        {
            if (!controller)
                controller = GetComponent<WormController>();
            return controller;
        }

        set
        {
            controller = value;
        }
    }

    public void Select()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
        characterInfo.transform.SetAsLastSibling();
    }

    public void Deselect()
    {
        characterInfo.GetComponent<UICharacterInfo>().ToggleActiveMarker();
    }

    private void OnDestroy()
    {
        Destroy(characterInfo);
    }

}
