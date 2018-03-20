using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
    public JSONManager.EnemyJSON m_stats;

    private Health m_healthBar;

    public void SetJSON(JSONManager.EnemyJSON j)
    {
        m_stats = j;
        m_healthBar = gameObject.GetComponent<Health>();
        m_healthBar.ownerClass = (Health.OwnerClass)m_stats.etype;
        m_healthBar.maxHealth = m_stats.hp;
        m_healthBar.currentHealth = m_stats.hp;
    }

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
