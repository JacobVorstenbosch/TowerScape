using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class BossAttackAI : RAINAction
{
    Animator anim;
    Animation ForLoop;
    bool attacking1;
    bool walkAnim;
    bool idle;

    RAINDecisionAttribute decision;
    RAIN.Core.CustomAIElement custom;

    

    public override void Start(RAIN.Core.AI ai)
    {

        base.Start(ai);
        anim = ai.Body.GetComponent<Animator>();
        if (actionName == "GolemAttack")
        {
            ai.WorkingMemory.SetItem("attacking", true);
            ai.WorkingMemory.SetItem("walking", false);
            walkAnim = false;
            attacking1 = true;  
        }
        else if (actionName == "GolemWalk")
        {
            ai.WorkingMemory.SetItem("attacking", false);
            ai.WorkingMemory.SetItem("walking", true);
            walkAnim = true;
            attacking1 = false;
        }
        anim.SetBool("walking", walkAnim);
        anim.SetBool("attacking", attacking1);
        //custom = ai.GetCustomElement("  ");

    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}