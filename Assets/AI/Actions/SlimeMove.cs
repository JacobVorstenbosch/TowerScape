using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SlimeMove : RAINAction
{
    Animator anim;
    IntakeGenerator ig;
    bool attacking;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        anim = ai.Body.GetComponent<Animator>();
        ig = ai.Body.GetComponentInChildren<IntakeGenerator>();
        ig.active = false;
        attacking = false;
        anim.SetBool("Attack", attacking);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ig.active = false;
        attacking = false;
        anim.SetBool("Attack", attacking);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        ig.active = false;
        attacking = false;
        anim.SetBool("Attack", attacking);
        base.Stop(ai);
    }
}