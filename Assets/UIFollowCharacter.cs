using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCharacter : MonoBehaviour {
    [SerializeField]
    public CharacterInstance character;

	void Start () {
		
	}
	
	void Update () {
        transform.position = Camera.main.WorldToScreenPoint(character.transform.position);  
    }
}
