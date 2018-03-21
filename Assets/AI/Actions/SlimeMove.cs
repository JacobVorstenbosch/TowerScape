using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SlimeMove : RAINAction
{
    Animator anim;
    bool attacking;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        anim = ai.Body.GetComponent<Animator>();
        attacking = false;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        attacking = false;
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}