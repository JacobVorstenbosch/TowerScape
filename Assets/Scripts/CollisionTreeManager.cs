using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTreeManager : MonoBehaviour {

    public Transform skeletonRef;
    public Transform weaponHand;
    public GameObject weapon;

    private List<Collider> colliders = new List<Collider>();

	// Use this for initialization
	void Start ()
    {
        RecursiveFillColliderList(skeletonRef);
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].isTrigger = true;
            colliders[i].transform.gameObject.AddComponent<CharacterColliderObject>();
        }

        GameObject equipedWeapon = Instantiate(weapon, weaponHand);
        equipedWeapon.layer = 10;
	}

    private void RecursiveFillColliderList(Transform node)
    {
        for (int i = 0; i < node.transform.childCount; i++)
            RecursiveFillColliderList(node.GetChild(i));
        Collider c = node.gameObject.GetComponent<Collider>();
        if (c)
            colliders.Add(c);
    }
}
