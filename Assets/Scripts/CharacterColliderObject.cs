using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderObject : MonoBehaviour {

    private CollisionTreeManager ctmParent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        print("THit");
    }

    void OnCollisionEnter(Collision col)
    {
        print("CHit");
    }

    public void SetCTMParent(CollisionTreeManager parent)
    {
        ctmParent = parent;
    }
}
