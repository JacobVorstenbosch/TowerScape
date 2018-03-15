using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    #region HEALTHMETHODS
    public enum OwnerClass { Enemy, Boss, Player };

    public Texture BarTexture;
    public static Texture StaticBarTexture;

    private float viewConeHalfAngle = 11.75f;
    private float maxPositionOffset = 2.5f;
    private float bgWidth = 1;
    private float bgHeight = 0.15f;
    private float fgWidth = 0.995f;
    private float fgHeight = 0.145f;
    private float minCircleFade = 5f;
    private float maxCircleFade = 7.5f;

    RawImage m_healthFG;
    RawImage m_healthBG;
    RawImage m_healthIG;
    GameObject m_goFG;
    GameObject m_goBG;
    GameObject m_goIG;
    GameObject m_goCanvas;
    public BuffManager buffManager;
    public float maxHealth;
    public float currentHealth;
    public float verticalOffset;
    public Vector3 fgColor;
    public Vector3 bgColor;
    public Vector3 ihColor;
    public OwnerClass ownerClass = OwnerClass.Enemy;
    public float invulnLength = 0.1f;
    private Vector3 origPos;
    private float origScale;

    private float preHitHpPCT;
    private float fgHalfWidth;
    private float impactChangeDelay = 5.0f;
    private float impactChangeTimer;
    
    // Use this for initialization
    void Start ()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        //find buffmanager
        Transform canvasParent = null; ;
        Vector3 offset = new Vector3(0, 0, 0);
        if (ownerClass == OwnerClass.Enemy)
        {
            m_goCanvas = new GameObject("NamePlate");
            m_goCanvas.transform.parent = transform;
            m_goCanvas.AddComponent<Canvas>().transform.localPosition = new Vector3(0, verticalOffset, 0);

            canvasParent = m_goCanvas.transform;
        }
        else if (ownerClass == OwnerClass.Boss)
        {
            canvasParent = GameObject.FindGameObjectWithTag("HUDCanvas").transform.Find("BossHealthRoot");
            offset = new Vector3(0, verticalOffset * canvasParent.childCount, 0);
            bgWidth = 505;
            bgHeight = 20;
            fgWidth = 500;
            fgHeight = 15;
        }
        else if (ownerClass == OwnerClass.Player)
        {
            canvasParent = GameObject.FindGameObjectWithTag("HUDCanvas").transform.Find("PlayerHealthRoot");
            bgWidth = 205;
            bgHeight = 20;
            fgWidth = 200;
            fgHeight = 15;
        }
        
        m_goBG = new GameObject("HealthBG");
        m_goBG.transform.parent = canvasParent;
        m_healthBG = m_goBG.AddComponent<RawImage>();
        m_healthBG.transform.localPosition = offset;
        m_healthBG.rectTransform.sizeDelta = new Vector2(bgWidth, bgHeight);
        m_healthBG.color = new Color(bgColor.x, bgColor.y, bgColor.z);

        m_goIG = new GameObject("HealthIG");
        m_goIG.transform.parent = canvasParent;
        m_healthIG = m_goIG.AddComponent<RawImage>();
        m_healthIG.transform.localPosition = offset;
        m_healthIG.rectTransform.sizeDelta = new Vector2(fgWidth, fgHeight);
        m_healthIG.color = new Color(ihColor.x, ihColor.y, ihColor.z);

        m_goFG = new GameObject("HealthFG");
        m_goFG.transform.parent = canvasParent;
        m_healthFG = m_goFG.AddComponent<RawImage>();
        m_healthFG.transform.localPosition = offset;
        m_healthFG.rectTransform.sizeDelta = new Vector2(fgWidth, fgHeight);
        m_healthFG.color = new Color(fgColor.x, fgColor.y, fgColor.z);

        origPos = m_healthFG.transform.position;
        preHitHpPCT = currentHealth / maxHealth;
        impactChangeTimer = impactChangeDelay;
        fgHalfWidth = fgWidth / 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!StaticBarTexture || m_healthFG.texture != StaticBarTexture)
        {
            if (BarTexture)
                StaticBarTexture = BarTexture;
            m_healthFG.texture = StaticBarTexture;
            m_healthBG.texture = StaticBarTexture;
            m_healthIG.texture = StaticBarTexture;
        }
        if (currentHealth <= 0 && ownerClass != OwnerClass.Player)
        {
            //TODO:
            //play anim, and do this once done
            //destroy everything we own
            Destroy(m_goBG);
            Destroy(m_goFG);
            Destroy(m_goIG);
            //destroy our object
            Destroy(gameObject);
        }
        else if (currentHealth <= 0 && ownerClass == OwnerClass.Player)
        {
            //home teleport, and reset
        }

        if (ownerClass == OwnerClass.Enemy)
        {
            //orient to camera
            m_goBG.transform.rotation = Camera.main.transform.rotation;
            m_goFG.transform.rotation = Camera.main.transform.rotation;
            m_goIG.transform.rotation = Camera.main.transform.rotation;

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
                m_healthIG.color = new Color(ihColor.x, ihColor.y, ihColor.z);
            }
            else
            {
                float coneFade = 1 - coneOffset / maxPositionOffset;
                float circleFade = 1 - (proj.magnitude - minCircleFade) / maxCircleFade;
                float alpha = coneFade * circleFade;
                m_healthFG.color = new Color(fgColor.x, fgColor.y, fgColor.z, alpha);
                m_healthBG.color = new Color(bgColor.x, bgColor.y, bgColor.z, alpha);
                m_healthIG.color = new Color(ihColor.x, ihColor.y, ihColor.z, alpha);
            }
        }

        //set width of fg based off of health pct
        float pct = currentHealth / maxHealth;
        m_healthFG.rectTransform.sizeDelta = new Vector2(pct * fgWidth, fgHeight);
        m_healthFG.transform.position = origPos + new Vector3(pct * fgWidth / 2 - fgHalfWidth,0,0);

        if (impactChangeTimer <= 0)
            preHitHpPCT = Mathf.MoveTowards(preHitHpPCT, pct, Time.deltaTime * 0.5f);
        else
            impactChangeTimer -= Time.deltaTime;

        if (preHitHpPCT < pct) preHitHpPCT = pct;
        
        m_healthIG.rectTransform.sizeDelta = new Vector2((preHitHpPCT - pct) * fgWidth, fgHeight);
        m_healthIG.transform.position = origPos + new Vector3(m_healthFG.rectTransform.rect.xMax + m_healthFG.rectTransform.rect.width / 2 + ((preHitHpPCT - pct) * fgWidth / 2 - fgHalfWidth), 0, 0);
    }

    public void OnSceneChange(UnityEngine.SceneManagement.Scene OldScene, UnityEngine.SceneManagement.Scene NewScene)
    {
        if (ownerClass == OwnerClass.Boss)
        {
            //destroy everything we own
            Destroy(m_goBG);
            Destroy(m_goFG);
            Destroy(m_goIG);
        }
    }

    /// <summary>
    /// The function used for dealing damage or healing a unit
    /// </summary>
    /// <param name="intake">A list of Intakes detailing the incoming damage/heal</param>
    public void ApplyIntake(ref List<Intake> intake)
    {
        for (int i = 0; i < intake.Count; i++)
        {
            if (intake[i].intakeType == Intake.IntakeType.DAMAGE)
            {
                float incoming = intake[i].GetModifiedAmmount();
                print("Took " + incoming + " " + intake[i].GetTypeString() + " Damage.");
                float newhp = currentHealth - incoming;
                float postHitHpPCT = newhp / maxHealth;
                if (preHitHpPCT <= postHitHpPCT) { preHitHpPCT = postHitHpPCT; impactChangeTimer = impactChangeDelay; }

                currentHealth = newhp;
                if (currentHealth <= 0) Destroy(gameObject);
                //applying invuln
                if (intake[i].intakeClass != Intake.IntakeClass.IGNORE_INVULN && incoming > 0)
                {
                    BuffManager.Invulnerability invuln = new BuffManager.Invulnerability();
                    invuln.startDuration = invulnLength;
                    buffManager.AddBuff(invuln);
                }
            }
            else if (intake[i].intakeType == Intake.IntakeType.HEAL)
            {
                float incoming = intake[i].GetModifiedAmmount();
                print("Took " + incoming + " " + intake[i].GetTypeString() + " Healing.");

                float newhp = currentHealth - incoming;
                float postHitHpPCT = newhp / maxHealth;
                preHitHpPCT = postHitHpPCT;
                if (currentHealth > maxHealth) currentHealth = maxHealth;
                currentHealth = newhp;
                if (currentHealth > maxHealth) currentHealth = maxHealth;
            }
        }
    }
    #endregion
}
