using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public JSONManager.EnemyJSON m_stats;

    public void SetJSON(JSONManager.EnemyJSON j)
    {
        m_stats = j;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
