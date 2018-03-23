using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartender : MonoBehaviour {

    bool active = false;
    float cooldown = 0.0f;
    public Health playerHp;

    public InteractionPane pane;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        cooldown -= Time.deltaTime;
		if (active && cooldown <= 0 && Input.GetAxis("Fire1") > 0.1f)
        {
            cooldown = 1.0f;
            if (playerHp.currentHealth > 10)
                playerHp.currentHealth -= 10;
            else
                pane.SetText("I think you've had enough.");
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerRoot"))
        {
            active = true;
            pane.SetActive(true);
            pane.SetText("Press A to order a drink.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerRoot"))
        {
            active = false;
            pane.SetText(" ");
            pane.SetActive(false);
        }
    }
}
