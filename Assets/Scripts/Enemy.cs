using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public struct EnemyJSON
    {
        public string type;    //used for spawning
        public string aitype;  //aitype used
        public float hp;       //max health
        public float dmg;      //damage per swing
        public float atsp;     //swings per second
        public float radius;   //used for spawning, allows for custom packing
    }

    public EnemyJSON m_stats;

    public void SetJSON(EnemyJSON j)
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
