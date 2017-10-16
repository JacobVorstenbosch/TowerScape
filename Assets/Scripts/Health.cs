using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

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
    public float maxHealth;
    public float currentHealth;
    public float verticalOffset;
    public Vector3 fgColor;
    public Vector3 bgColor;

    // Use this for initialization
    void Start ()
    {
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
        //orient to camera
        m_goBG.transform.rotation = Camera.main.transform.rotation;
        m_goFG.transform.rotation = Camera.main.transform.rotation;

        Vector3 proj = Vector3.Project(transform.position, Camera.main.transform.forward);
        float offsetMag = (transform.position - proj).magnitude;
        
        //calculate the opposite side of the triangle
        //tan(theta) = opposite / adjacent
        //adjacent = proj.mag
        //opposite = tan * adjacent
        float coneWidth = Mathf.Tan(viewConeHalfAngle * (Mathf.PI / 180)) * proj.magnitude;

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
}
