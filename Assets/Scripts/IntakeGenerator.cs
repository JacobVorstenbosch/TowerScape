using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntakeGenerator : MonoBehaviour
{
    //intake values
    public Intake.IntakeType itype;
    [Tooltip("Passive and Active are for healing only, Pure is for unmodified intake.")]
    public Intake.IntakeClass iclass;
    public float ammount;

    public bool active = false;
    
    [Tooltip("Auto Assigned")]
    public BuffManager buffManager;


    public List<Intake> GetBuffedIntakeList()
    {
        Intake intake = new Intake();
        intake.ammount = ammount;
        intake.intakeType = itype;
        intake.intakeClass = iclass;
        intake.origin = buffManager.gameObject;

        List<Intake> ilist = new List<Intake>();
        ilist.Add(intake);

        int start = 0;
        int len = 0;
        if (itype == Intake.IntakeType.DAMAGE)
        {
            start = buffManager.GetActivatorFirstElementIndex(BuffManager.Buff.Activator.ON_DMG_DEAL);
            len = buffManager.GetActivatorLength(BuffManager.Buff.Activator.ON_DMG_DEAL);
        }
        else if (itype == Intake.IntakeType.HEAL)
        {
            start = buffManager.GetActivatorFirstElementIndex(BuffManager.Buff.Activator.ON_HEAL_DEAL);
            len = buffManager.GetActivatorLength(BuffManager.Buff.Activator.ON_HEAL_DEAL);
        }

        for (int i = start; i < start + len; i++)
            buffManager.buffs[i].Trigger(ilist);

        return ilist;
    }
}


/// <summary>
/// Intake class represents any modificaton to health.
/// </summary>
public class Intake
{
    public struct Modifier
    {
        public enum ModifierType { ADDITIVE, MULTIPLICATIVE };
        public ModifierType modType;
        public float val;
        public Modifier(ModifierType modifierType, float value)
        {
            modType = modifierType;
            val = value;
        }
    }
    public enum IntakeType { DAMAGE, HEAL };
    public IntakeType intakeType;
    public enum IntakeClass { PURE, PASSIVE, ACTIVE, PHYSICAL, FIRE, ICE, CHAOS, IGNORE_INVULN };
    public IntakeClass intakeClass;
    public float ammount;
    public GameObject origin;
    protected List<Modifier> modifiers = new List<Modifier>();
    private int index;
    //note values must be in % format, ie 15% is 0.15 and -15% is -0.15
    public void AddModifier(Modifier m)
    {
        if (m.modType == Modifier.ModifierType.ADDITIVE)
            modifiers.Insert(index, m);
        else if (m.modType == Modifier.ModifierType.MULTIPLICATIVE)
        {
            for (int i = 0; i < index; i++)
            {
                if (modifiers[i].val < m.val)
                {
                    modifiers.Insert(i, m);
                    index++;
                    return;
                }
            }
            modifiers.Insert(0, m);
            index++;
        }
    }
    public float GetModifiedAmmount()
    {
        float additiveScalar = 1;
        float multiplicitaveScalar = 1;
        //calculate additive scalar
        for (int i = 0; i < index; i++)
            additiveScalar += modifiers[i].val;
        //calculate multiplicitve scalar
        for (int i = index; i < modifiers.Count; i++)
            multiplicitaveScalar *= 1 + modifiers[i].val;

        float final = ammount * additiveScalar * multiplicitaveScalar;

        return final > 0 ? final : 0;
    }
    public string GetTypeString()
    {
        if (intakeClass == IntakeClass.PURE)
            return "PURE";
        else if (intakeClass == IntakeClass.PASSIVE)
            return "PASSIVE";
        else if (intakeClass == IntakeClass.ACTIVE)
            return "ACTIVE";
        else if (intakeClass == IntakeClass.PHYSICAL)
            return "PHYSICAL";
        else if (intakeClass == IntakeClass.FIRE)
            return "FIRE";
        else if (intakeClass == IntakeClass.ICE)
            return "ICE";
        else if (intakeClass == IntakeClass.CHAOS)
            return "CHAOS";
        else if (intakeClass == IntakeClass.IGNORE_INVULN)
            return "IGNORE_INVULN";
        else return "UNKOWN";
    }
    public int GetNumModifiers()
    {
        return modifiers.Count;
    }
}