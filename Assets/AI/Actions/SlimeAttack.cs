using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SlimeAttack : RAINAction
{
    Animator anim;
    bool attacking;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        anim = anim.GetComponent<Animator>();
        attacking = anim.GetBool("Attack");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        attacking = true;
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}