using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderObject : MonoBehaviour
{

    private CollisionTreeManager ctmParent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    print(other.tag);
    //    if (other.CompareTag("IntakeSource"))
    //    {
    //        IntakeGenerator intake = other.gameObject.GetComponent<IntakeGenerator>();
    //        if (!intake || intake.active == false)
    //            return;
    //        List<Intake> ilist = intake.GetBuffedIntakeList();
    //        ctmParent.OnIntake(ref ilist);
    //    }
    //}

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("IntakeSource") || (other.CompareTag("Enemy") && tag.Equals("Player")))
        {
            IntakeGenerator intake = other.gameObject.GetComponent<IntakeGenerator>();
            if (!intake || intake.active == false)
                return;
            List<Intake> ilist = intake.GetBuffedIntakeList();
            ctmParent.OnIntake(ref ilist);
        }
    }

    public void SetCTMParent(CollisionTreeManager parent)
    {
        ctmParent = parent;
    }
}
