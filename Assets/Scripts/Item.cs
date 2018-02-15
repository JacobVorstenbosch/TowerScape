using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Color borderCol;
    public Sprite image;
    public string itemName;
    public string tooltip;
    public List<float> values;
    public List<BuffManager.Buff> buffList;


    //items will be stored in the following format:
    //borderCol,image?,itemName,tooltip,values[0],...values[max]
    public void FromFile(string itemstring)
    {
        print("Feature not done.");
    }

	// Use this for initialization
	void Start () {

	}

    public void Equip()
    {
        //unapply and destroy actual weapon


        //actual weapon stuff

        //apply buffs

        //move item to equip slot, swap if necessiary
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
