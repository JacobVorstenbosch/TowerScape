using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTreeManager : MonoBehaviour {

    [Tooltip("BuffManager must always be on the root gameobject.")]
    public BuffManager buffManager;
    [Tooltip("Tag to be assigned to all colliders.")]
    public string assignTag = "Untagged";
    public Transform skeletonRef;
    public Transform weaponHand;
    public GameObject weapon;
    public Health health;
    public float invulnLength = 1f;
    private float currentInvuln = 0f;
    private bool invuln;

    private List<Collider> colliders = new List<Collider>();

	// Use this for initialization
	void Start ()
    {
        RecursiveFillColliderList(skeletonRef);
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].isTrigger = true;
            CharacterColliderObject cco = colliders[i].transform.gameObject.AddComponent<CharacterColliderObject>();
            cco.SetCTMParent(this);
        }

        if (weapon)
        {
            GameObject equipedWeapon = Instantiate(weapon, weaponHand);
            IntakeGenerator ig = equipedWeapon.GetComponent<IntakeGenerator>();
            ig.buffManager = buffManager;
            equipedWeapon.layer = 10;
        }
	}

    private void RecursiveFillColliderList(Transform node)
    {
        for (int i = 0; i < node.transform.childCount; i++)
            RecursiveFillColliderList(node.GetChild(i));
        Collider[] c = node.gameObject.GetComponents<Collider>();
        if (c.Length > 0)
        {
            for (int i = 0; i < c.Length; i++)
                colliders.Add(c[i]);
            c[0].gameObject.tag = assignTag;
        }
    }

    public void OnIntake(ref List<Intake> intake)
    {
        //apply our onintake buffs
        int originalIntakeCount = intake.Count; //so we dont infinitley proc additional effects
        for (int i = 0; i < originalIntakeCount; i++)
        {
            int start = 0;
            int len = 0;
            if (intake[i].intakeType == Intake.IntakeType.DAMAGE)
            {
                start = buffManager.GetActivatorFirstElementIndex(BuffManager.Buff.Activator.ON_DMG_INTAKE);
                len = buffManager.GetActivatorLength(BuffManager.Buff.Activator.ON_DMG_INTAKE);
            }
            else if (intake[i].intakeType == Intake.IntakeType.HEAL)
            {
                start = buffManager.GetActivatorFirstElementIndex(BuffManager.Buff.Activator.ON_HEAL_INTAKE);
                len = buffManager.GetActivatorLength(BuffManager.Buff.Activator.ON_HEAL_INTAKE);
            }

            for (int j = start; j < start + len; j++)
                buffManager.buffs[j].Trigger(intake, originalIntakeCount);
        }
        health.ApplyIntake(ref intake);
    }
}
