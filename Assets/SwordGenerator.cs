using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordGenerator: MonoBehaviour {

    public GameObject[] HiltComponents;
    public GameObject[] GuardComponents;
    public GameObject[] BladeComponents;

    public void SwordGeneratorC(int hiltIndex, int guardIndex, int bladeIndex )
    {
        GenerateHiltComponent(hiltIndex);
        GenerateGuardComponent(guardIndex);
        GenerateBladeComponent(bladeIndex);
    }
    
    private void GenerateHiltComponent(int componentIndex)
    {
        Instantiate(HiltComponents[componentIndex], transform);
    }
    private void GenerateGuardComponent(int componentIndex)
    {
        Instantiate(GuardComponents[componentIndex], transform);
    }
    private void GenerateBladeComponent(int componentIndex)
    {
        Instantiate(BladeComponents[componentIndex], transform);
    }

    void Start()
    {
        SwordGeneratorC(0, 0, 0);
    }

}
