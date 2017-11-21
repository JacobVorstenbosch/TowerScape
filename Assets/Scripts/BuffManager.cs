using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    #region MANAGER
    private List<Intake> emptyIntakeList = new List<Intake>();
    public List<Buff> buffs = new List<Buff>();
    private int[] indicies = new int[5];//number must always be the size of activators

    /// <summary>
    /// Returns the number of buffs that use the activator passed
    /// </summary>
    /// <param name="a">Activator to check</param>
    /// <returns>Returns the number of buffs that use the activator passed</returns>
    public int GetActivatorLength(Buff.Activator a)
    {
        int index = (int)a + 1 >= indicies.Length ? indicies.Length - 1 : (int)a + 1;
        return indicies[index] - indicies[(int)a];
    }

    /// <summary>
    /// Returns the first element that uses the activator passed
    /// </summary>
    /// <param name="a">The activator to check for</param>
    /// <returns>The index of the first element that uses the activator passed</returns>
    public int GetActivatorFirstElementIndex(Buff.Activator a)
    {
        return indicies[(int)a];
    }

    /// <summary>
    /// Activates ON_TICK activators and removes any buff out of time
    /// </summary>
    private void Update()
    {
        //if (buffs.Count != 0)
        //{
        //    string debug = "a\ts\tl\n";
        //    for (int i =  0; i < indicies.Length; i++)
        //        debug += i.ToString() + "\t" + GetActivatorFirstElementIndex((Buff.Activator)i).ToString() + "\t" + GetActivatorLength((Buff.Activator)i).ToString() + "\n";
        //    print(debug);
        //}

        int otstart = GetActivatorFirstElementIndex(Buff.Activator.ON_TICK);
        int otlen = GetActivatorLength(Buff.Activator.ON_TICK);
        for (int i = otstart, s = otstart + otlen; i < s; i++)
            buffs[i].Trigger(emptyIntakeList, 0);
        
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].durationRemaining -= Time.deltaTime;
            buffs[i].OnUpdate();
            if (buffs[i].durationRemaining <= 0)
            {
                for (int j = (int)buffs[i].activator + 1; j < 5; j++)//J MUST BE LESS THAN  NUMBER OF ACTIVATORS
                    indicies[j]--;
                buffs[i].OnEnd();
                buffs.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Adds a buff into the list, and calls the OnStart function
    /// </summary>
    /// <param name="b">the buff to add</param>
    public void AddBuff(Buff b)
    {
        b.OnStart();
        buffs.Insert(indicies[(int)b.activator], b);
        for (int i = (int)b.activator + 1; i < indicies.Length; i++)
            indicies[i]++;
    }

    #endregion
    
    #region BUFFS
    public abstract class Buff
    {
        public enum Effect { POSITIVE, NEGATIVE };
        public enum Activator { ON_DMG_INTAKE, ON_DMG_DEAL, ON_HEAL_INTAKE, ON_TICK, ON_HEAL_DEAL };

        public string name;
        public Effect effect;
        public Activator activator;
        public bool hidden = false;
        public bool passive = false;
        public float startDuration;
        public float durationRemaining;

        public abstract void OnStart();
        public abstract void OnEnd();
        public abstract void OnUpdate();

        public abstract void Trigger(List<Intake> intake, int orignalIntakeCount = 1);
    }

    /* BASE BUFF/DEBUFF CLASS
    public class BuffName : Buff
    {
        public override void OnStart()
        {
            //set duration or passive
            durationRemaining = startDuration;
            //set activator
            //apply particle effects
        }

        public override void OnEnd()
        {
            //remove particle effects
        }

        public override void OnUpdate()
        {
            //update particle effects
        }

        public override void Trigger(List<Intake> intake = null, int originalIntakeCount = 1)
        {
            //apply buff to intake
        }
    }
    */

    public class Invulnerability : Buff
    {
        public override void OnStart()
        {
            durationRemaining = startDuration;
            activator = Activator.ON_DMG_INTAKE;
            name = "Invulnerability";
            hidden = true;
            //apply particles
        }

        public override void OnEnd()
        {
            //destroy particles
        }

        public override void OnUpdate()
        {
        }

        public override void Trigger(List<Intake> intake, int originalIntakeCount = 1)
        {
            for (int i = 0; i < intake.Count; i++)
            {
                if (intake[i].intakeType == Intake.IntakeType.DAMAGE && intake[i].intakeClass != Intake.IntakeClass.IGNORE_INVULN)
                    intake[i].AddModifier(new Intake.Modifier(Intake.Modifier.ModifierType.MULTIPLICATIVE, -1));
            }
        }
    }
    #endregion
}
