using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    Team[] teams;
    
	void Awake () {
		if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}

}
