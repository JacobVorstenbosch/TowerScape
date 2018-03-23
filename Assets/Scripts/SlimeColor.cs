using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeColor : MonoBehaviour {

    private Vector4 color = new Vector4(0, 0, 0, 0);

    // Use this for initialization
    void Start () {
        float ranVal = Random.Range(0, 255);
        Color c = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1, 1);
        color = new Vector4(c.r, c.g, c.b, c.a);
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
