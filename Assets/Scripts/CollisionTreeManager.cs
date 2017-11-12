using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTreeManager : MonoBehaviour {

    public Transform skeletonRef;
    public Transform weaponHand;
    public GameObject weapon;
    public Health health;
    public float invulnLength = 1f;
    private float currentInvuln = 0f;

    private List<Collider> colliders = new List<Collider>();

	// Use this for initialization
	void Start ()
    {
        RecursiveFillColliderList(skeletonRef);
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].isTrigger = true;
            CharacterColliderObject cco = colliders[i].transform.gameObject.AddComponent<CharacterColliderObject>();
            cco.health = health;
        }

        if (weapon)
        {
            GameObject equipedWeapon = Instantiate(weapon, weaponHand);
            equipedWeapon.layer = 10;
        }
	}

    private void RecursiveFillColliderList(Transform node)
    {
        for (int i = 0; i < node.transform.childCount; i++)
            RecursiveFillColliderList(node.GetChild(i));
        Collider[] c = node.gameObject.GetComponents<Collider>();
        if (c.Length > 0)
            for (int i = 0; i < c.Length; i++)
                colliders.Add(c[i]);
    }
}
