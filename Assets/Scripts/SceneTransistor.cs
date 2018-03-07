using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransistor : MonoBehaviour {

    [Tooltip("Animation to be played before the transition actually happens")]
    public Animation PlayerTransitionAnim;

    [Tooltip("Scene to transition to.")]
    public string sceneName;
    [Tooltip("Controller axis to actually transition.")]
    public string controllerAxis = "";
    [Tooltip("Key used as backup to actually transition.")]
    public KeyCode transistionKey;
    [Tooltip("Interaction text.")]
    public string interactionText;
    [Tooltip("Canvas interaction pane.")]
    public InteractionPane pane;

    private bool playerInArea = false;

	// Use this for initialization
	void Start () {
        if (pane == null)
            pane = GameObject.FindGameObjectWithTag("HUDCanvas").transform.Find("InteractionPane").GetComponent<InteractionPane>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerInArea && ((transistionKey != KeyCode.None && Input.GetKeyDown(transistionKey)) || (controllerAxis != "" && Input.GetAxis(controllerAxis) > 0.1)))
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("PlayerRoot"))
        {
            pane.SetActive(true);
            pane.SetText(interactionText);
            playerInArea = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.CompareTag("PlayerRoot"))
        {
            pane.SetActive(false);
            playerInArea = false;
        }
    }
}
