using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGenerator: MonoBehaviour {
    
    public GameObject player;

    public Material bladeMat;

    public GameObject[] HiltComponents;
    public GameObject[] GuardComponents;
    public GameObject[] BladeComponents;

    private CollisionTreeManager playerCTM;
    private BuffManager playerBM;
    private PlayerScript playerS;
    private int rarity = 0;
    private int typeChance = 0;

    private Vector4 AuraColor = new Vector4 (0,0,0,0);

    [Tooltip("Must be length of 5\nMust add up to 1\nFirst element is Common, last is Legendary")]
    public float[] RarityProbabilities;
    [Tooltip("Must be length of 5\nMust add up to 1\nPhys,Chaos,Fire,Ice,Pure")]
    public float[] ElementProbabilities;

    private GameObject sword;

    public GameObject Generate()
    {
        sword = Generate(Random.Range(0, HiltComponents.Length), Random.Range(0, GuardComponents.Length), Random.Range(0, BladeComponents.Length));
        //bladeMat = GetComponent<Renderer>().material;
        //bladeMat.SetColor("_ColorR", new Vector4(50, 50, 50, 0.7f));
        //bladeMat.SetColor("_Color2", new Vector4(50, 50, 50, 0.7f));
        return sword;

    }

    public GameObject Generate(int hiltIndex, int guardIndex, int bladeIndex )
    {
        Destroy(sword);

        Transform blade = GenerateBladeComponent(bladeIndex);
        sword = blade.gameObject;
        GenerateHiltComponent(hiltIndex, blade);
        GenerateGuardComponent(guardIndex, blade);

        IntakeGenerator ig = blade.GetComponent<IntakeGenerator>();

        float ranval = Random.value;
        for (; typeChance < ElementProbabilities.Length; typeChance++)
        {
            ranval -= ElementProbabilities[typeChance];
            if (ranval <= 0)
                break;
        }
        //Chaos = 6, Fire = 4, Ice = 5,, Pure = 0, phys = 3
        if(typeChance == 1)
        {
            ig.iclass = Intake.IntakeClass.PHYSICAL;
        }
        if (typeChance == 2)
        {
            ig.iclass = Intake.IntakeClass.CHAOS;
        }
        if (typeChance == 3)
        {
            ig.iclass = Intake.IntakeClass.FIRE;
        }
        if (typeChance == 4)
        {
            ig.iclass = Intake.IntakeClass.ICE;
        }
        if (typeChance == 5)
        {
            ig.iclass = Intake.IntakeClass.PURE;
        }


        //setting blade aura color to match rarity ~~ can we modified to match elemental type
        if (ig.iclass == Intake.IntakeClass.PHYSICAL)
        {
            AuraColor = new Vector4(50, 50, 50, 0.7f);
        }

        if (ig.iclass == Intake.IntakeClass.CHAOS)
        {
            AuraColor = new Vector4(4, 0, 50, 0.7f);
        }

        if (ig.iclass == Intake.IntakeClass.FIRE)
        {
            AuraColor = new Vector4(255, 0, 0, 0.7f);
        }

        if (ig.iclass == Intake.IntakeClass.ICE)
        {
            AuraColor = new Vector4(0, 255, 255, 0.7f);
        }

        if (ig.iclass == Intake.IntakeClass.PURE)
        {
            AuraColor = new Vector4(255, 255, 255, 0.7f);
        }

        bladeMat = sword.GetComponent<Renderer>().material;
        bladeMat.SetColor("_ColorR", AuraColor);
        bladeMat.SetColor("_Color2", AuraColor);

        playerCTM.weapon = ig.gameObject;
        ig.buffManager = playerBM;
        playerCTM.weapon.layer = 10;

        playerS.UpdateIG(ig);

        return blade.gameObject;
    }
    
    private void GenerateHiltComponent(int componentIndex, Transform parent)
    {
        GameObject go = Instantiate(HiltComponents[componentIndex], parent);
    }
    private void GenerateGuardComponent(int componentIndex, Transform parent)
    {
        GameObject go = Instantiate(GuardComponents[componentIndex], parent);
    }
    private Transform GenerateBladeComponent(int componentIndex)
    {
        GameObject blade = Instantiate(BladeComponents[componentIndex], transform.parent);
        blade.tag = "IntakeSource";
        blade.transform.localPosition = transform.localPosition;
        blade.transform.localRotation = transform.localRotation;
        IntakeGenerator ig = blade.AddComponent<IntakeGenerator>();
        ig.itype = Intake.IntakeType.DAMAGE;
        //can be phys, ice, fire, or chaos or pure i geuss cus we have no mitigation
        ig.iclass = Intake.IntakeClass.PHYSICAL;
        //random this, based of rarity common = 17.5 - 22.5, uncommon = 20-25, rare = 22.5-27.5, epic = 25-30, leggo = 27.5 - 32.5
        //for each line in the file
        float ranval = Random.value;
        for (; rarity < RarityProbabilities.Length; rarity++)
        {
            ranval -= RarityProbabilities[rarity];
            if (ranval <= 0)
                break;
        }
        ig.ammount = 17.5f + rarity * 2.5f + Random.Range(0f,5f);
        return blade.transform;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRoot");
        playerCTM = player.GetComponent<CollisionTreeManager>();
        playerBM = player.GetComponent<BuffManager>();
        playerS = player.GetComponent<PlayerScript>();
        sword = Generate(Random.Range(0, HiltComponents.Length), Random.Range(0, GuardComponents.Length), Random.Range(0, BladeComponents.Length));
    }

}
