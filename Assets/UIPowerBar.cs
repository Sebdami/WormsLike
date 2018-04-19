using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerBar : MonoBehaviour {
    public Image fillImage;
	
	public void UpdateFillValue(float value)
    {
        if(!fillImage)
            fillImage = transform.GetChild(1).GetComponent<Image>();
        fillImage.fillAmount = value;
    }
}
