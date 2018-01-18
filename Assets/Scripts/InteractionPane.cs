using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPane : MonoBehaviour {

    public UnityEngine.UI.RawImage MainBanner;
    public UnityEngine.UI.RawImage BackBanner;
    public UnityEngine.UI.Text TextBanner;

	// Use this for initialization
	void Start () {
        SetActive(false);	
	}

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetText(string text)
    {
        TextBanner.text = text;
    }

}
