using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    
    #region INTAKEMETHODS

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
        protected float ammount;
        protected List<Modifier> modifiers = new List<Modifier>();
        private int index;
        //note values must be in % format, ie 15% is 0.15 and -15% is -0.15
        public void AddModifier(Modifier m)
        {
            if (m.modType == Modifier.ModifierType.ADDITIVE)
                modifiers.Insert(index, m);
            else if (m.modType == Modifier.ModifierType.MULTIPLICATIVE)
                for (int i = 0; i < index; i++)
                    if (modifiers[i].val < m.val)
                    {
                        modifiers.Insert(i, m);
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
    }

    public class Heal : Intake
    {
        public enum HealClass { PURE, PASSIVE, ACTIVE };
        public HealClass healClass;
        public GameObject origin;
    }

    public class Damage : Intake
    {
        public enum DamageClass { PURE, PHYSICAL, FIRE, ICE, CHAOS, IGNORE_INVULN };
        public DamageClass damageClass;
        public GameObject origin;
    }
    #endregion

    #region HEALTHMETHODS
    public enum OwnerClass { Enemy, Boss, Player };

    const float viewConeHalfAngle = 11.75f;
    const float maxPositionOffset = 2.5f;
    const float bgWidth = 1;
    const float bgHeight = 0.1f;
    const float fgWidth = 0.995f;
    const float fgHeight = 0.095f;
    const float minCircleFade = 5f;
    const float maxCircleFade = 7.5f;

    RawImage m_healthFG;
    RawImage m_healthBG;
    GameObject m_goFG;
    GameObject m_goBG;
    GameObject m_goCanvas;
    BuffManager buffManager;
    public float maxHealth;
    public float currentHealth;
    public float verticalOffset;
    public Vector3 fgColor;
    public Vector3 bgColor;
    public OwnerClass ownerClass = OwnerClass.Enemy;

    // Use this for initialization
    void Start ()
    {
        //find buffmanager
        if (!(ownerClass == OwnerClass.Enemy))
            return;
        m_goCanvas = new GameObject("NamePlate");
        m_goCanvas.transform.parent = transform;
        Canvas canvas = m_goCanvas.AddComponent<Canvas>();
        canvas.transform.localPosition = new Vector3(0, verticalOffset, 0);
        
        m_goBG = new GameObject("HealthBG");
        m_goBG.transform.parent = m_goCanvas.transform;
        m_healthBG = m_goBG.AddComponent<RawImage>();
        m_healthBG.transform.localPosition = new Vector3(0, 0, 0);
        m_healthBG.rectTransform.sizeDelta = new Vector2(bgWidth, bgHeight);
        m_healthBG.color = new Color(bgColor.x, bgColor.y, bgColor.z);

        m_goFG = new GameObject("HealthFG");
        m_goFG.transform.parent = m_goCanvas.transform;
        m_healthFG = m_goFG.AddComponent<RawImage>();
        m_healthFG.transform.localPosition = new Vector3(0, 0, 0);
        m_healthFG.rectTransform.sizeDelta = new Vector2(fgWidth, fgHeight);
        m_healthFG.color = new Color(fgColor.x, fgColor.y, fgColor.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!(ownerClass == OwnerClass.Enemy))
            return;
        //orient to camera
        m_goBG.transform.rotation = Camera.main.transform.rotation;
        m_goFG.transform.rotation = Camera.main.transform.rotation;

        Vector3 proj = Vector3.Project(transform.position, Camera.main.transform.forward);
        float offsetMag = (transform.position - proj).magnitude;
        
        //calculate the opposite side of the triangle
        //tan(theta) = opposite / adjacent
        //adjacent = proj.mag
        //opposite = tan * adjacent
        float coneWidth = Mathf.Tan(viewConeHalfAngle * Mathf.Deg2Rad) * proj.magnitude;

        //offset from cone
        float coneOffset = offsetMag - coneWidth;
        //if coneOffset is negative we are in the cone itself
        if (coneOffset < 0 && proj.magnitude < minCircleFade)
        {
            m_healthFG.color = new Color(fgColor.x, fgColor.y, fgColor.z);
            m_healthBG.color = new Color(bgColor.x, bgColor.y, bgColor.z);
        }
        else
        {
            float coneFade = 1 - coneOffset / maxPositionOffset;
            float circleFade = 1 - (proj.magnitude - minCircleFade) / maxCircleFade;
            float alpha = coneFade * circleFade;
            m_healthFG.color = new Color(fgColor.x, fgColor.y, fgColor.z, alpha);
            m_healthBG.color = new Color(bgColor.x, bgColor.y, bgColor.z, alpha);
        }

        //set width of fg based off of health pct
        m_healthFG.rectTransform.sizeDelta = new Vector2(currentHealth / maxHealth * fgWidth, fgHeight);
    }

    public void ApplyIntake(ref List<Intake> intake)
    {
        for (int i = 0; i < intake.Count; i++)
        {

        }
    }
    #endregion
}
