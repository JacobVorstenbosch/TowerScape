using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSelector : MonoBehaviour {

    JSONManager m_jsonManager;

	// Use this for initialization
	void Start () {
        m_jsonManager = GameObject.FindGameObjectWithTag("JSONManager").GetComponent<JSONManager>();
        m_jsonManager.currentFloor = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
