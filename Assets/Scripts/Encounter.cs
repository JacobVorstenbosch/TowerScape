using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour {

    public Transform trianglePoint1;
    public Transform trianglePoint2;
    public Transform trianglePoint3;

    public int floorNumber;

    private JSONManager m_jsonManager;
    private JSONManager.EncounterJSON m_encounter;


    public GameObject baseEnemy;

	// Use this for initialization
	void Start ()
    {
        m_jsonManager = GameObject.FindGameObjectWithTag("JSONManager").GetComponent<JSONManager>();
        m_jsonManager.currentFloor++;
        floorNumber = m_jsonManager.currentFloor;
        m_encounter = m_jsonManager.GetEncounterByFloor(floorNumber);
        SpawnEncounter();
    }
    

    void SpawnEncounter()
    {
        //define triangle using the 3 points, where 1 points to player
        //always favour middle for spawns
        //back wall is defined by the corners 2 and 3
        Vector3 middle = (trianglePoint1.position + trianglePoint2.position + trianglePoint3.position) / 3;
        //float sin60 = Mathf.Sin(60 * Mathf.Deg2Rad);
        Vector3[] prevSet = new Vector3[3];
        Vector3[] pointNormals = new Vector3[3];
        for (int i = 0; i < 3; i++)
            prevSet[i] = middle;
        pointNormals[0] = (trianglePoint1.position - middle).normalized;
        pointNormals[1] = (trianglePoint2.position - middle).normalized;
        pointNormals[2] = (trianglePoint3.position - middle).normalized;
        float prevRadius = 0;


        for (int i = 0; i < m_encounter.counts.Length; i++)
        {
            string debugString = "";
            int count = m_encounter.counts[i];
            debugString += "Detected " + count + " enemies to create.\n";
            float radius = EnemyByName(m_encounter.enemies[i]).radius;

            //resize the triangle if radius is bigger
            float mod = radius + prevRadius;
            for (int j = 0; j < 3; j++)
                prevSet[j] += pointNormals[j] * mod;
             

            float triangleLength = (prevSet[0] - prevSet[1]).magnitude;
            debugString += "Previous triangle length: " + triangleLength + "\n";
            int perSideCount = Mathf.CeilToInt(count / 2);
            float halfCount = count / 2;

            //check if we need more space
            //radii required appears to be FloorToInt(count / 2) * 4
            float requiredLength;
            if (count % 2 == 0)
                requiredLength = (halfCount + 1f) * radius;
            else
                requiredLength = halfCount * radius;

            debugString += "Required triangle length: " + requiredLength + "\n";
            if (triangleLength < requiredLength)
            {
                float halfoffsetsqaured = Mathf.Pow((requiredLength - triangleLength) / 2, 2f);
                //calculate the required pointnormal scale
                float scalar = Mathf.Sqrt(halfoffsetsqaured * 2); //c^2 = a^2 + b^2 where a == b
                //scale the triangle
                for (int j = 0; j < 3; j++)
                    prevSet[j] += pointNormals[j] * scalar;

                debugString += "New triangle length: " + (prevSet[0] - prevSet[1]).magnitude + "\n";
            }
            else
            {
                debugString += "No need to resize triangle, requiredLength being set to triangleLength.\n";
                requiredLength = triangleLength;
            }

            if (count % 2 == 0)//even
            {
                float spawnGap = requiredLength / (perSideCount + 1);
                Vector3 leftSide = (prevSet[1] - prevSet[0]).normalized;
                Vector3 rightSide = (prevSet[2] - prevSet[0]).normalized;
                for (int j = 1; j <= perSideCount; j++)
                {
                    debugString += "Spawning enemy using offset: " + spawnGap * j + "\n";
                    //spawn on both sides
                    SpawnEnemy(m_encounter.enemies[i], prevSet[0] + leftSide * (spawnGap * j));
                    SpawnEnemy(m_encounter.enemies[i], prevSet[0] + rightSide * (spawnGap * j));
                }
            }
            else//odd
            {
                SpawnEnemy(m_encounter.enemies[i], prevSet[0]);
                if (count == 1)//single enemy
                {
                    debugString += "Only one enemy, were done this set.\n";
                    print(debugString);
                    continue;
                }
                float spawnGap = requiredLength / perSideCount;
                //calculate triange sides
                Vector3 leftSide = (prevSet[1] - prevSet[0]).normalized;
                Vector3 rightSide = (prevSet[2] - prevSet[0]).normalized;
                debugString += "It's odd so it behaves oddly, " + perSideCount + " per side including front.\n";
                for (int j = 1; j <= perSideCount; j++)
                {
                    //spawn on both sides
                    debugString += "Spawning enemy using offset: " + spawnGap * j + "\n";
                    SpawnEnemy(m_encounter.enemies[i], prevSet[0] + leftSide * (spawnGap * j));
                    SpawnEnemy(m_encounter.enemies[i], prevSet[0] + rightSide * (spawnGap * j));
                }
                

            }

            print(debugString);

            //increment prevSet triangle
            //for (int j = 0; j < 3; j++)
            //    prevSet[j] += pointNormals[j] * radius;
            prevRadius = radius;
        }
    }

    JSONManager.EnemyJSON EnemyByName(string e)
    {
        return m_jsonManager.GetEnemyByName(e);
    }

    void SpawnEnemy(string e, Vector3 p)
    {
        GameObject go = Instantiate(Resources.Load("EnemyModels/" + e) as GameObject, p, Quaternion.identity);
        go.AddComponent<Enemy>();
        go.GetComponent<Enemy>().SetJSON(EnemyByName(e));
    }
}
