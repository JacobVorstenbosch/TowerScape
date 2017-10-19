using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour {

    public struct EncounterJSON
    {
        public string[] enemies;
        public int[] counts;
    }

    public Transform trianglePoint1;
    public Transform trianglePoint2;
    public Transform trianglePoint3;

    public int floorNumber;

    private EncounterJSON m_encounter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Attempt to spawn in a triangle patter where the backside is unused, in a circular pattern
    //index 0 would be in the middle, while the last index would be the outer edge
    void SpawnEncounter()
    {
        //define triangle using the 3 points, where 1 points to player
        //always favour middle for spawns
        //back wall is defined by the corners 2 and 3

        Vector3 middle = (trianglePoint1.position + trianglePoint2.position + trianglePoint3.position) / 3;
        float sin60 = Mathf.Sin(60 * Mathf.Deg2Rad);
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
            int count = m_encounter.counts[i];
            float radius = EnemyByName(m_encounter.enemies[i]).m_stats.radius;

            //easy method?
            //check if i can pack the required spawns in the triangle
            //if not expand by minimum ammount
            //calculate midpoints/end points based on even/odd
            //spawn

            float triangleLength = (prevSet[0] - prevSet[1]).magnitude;
            int perSideCount = Mathf.CeilToInt(count / 2);
            //check if we need more space
            //1 = 0? only at prevSet[0] offset radius
            //2 = 4
            //3 = 4
            //4 = 8
            //5 = 8
            //appears to be FloorToInt(count / 2) * 4

            if (count % 2 == 0)//even
            {
                //calculate number of enemies on each side of triangle
                int stack = count / 2;

                //base triangle length is 2radius since equalaterial
                //calculate count = 2 half height
                float c2halfHeight = sin60 * radius;
            }
            else//odd
            {
                if (count == 1)//single enemy
                {
                    continue;
                }


            }
            //increment prevSet triangle
            for (int j = 0; j < 3; j++)
                prevSet[j] += pointNormals[j] * radius;
            prevRadius = radius;
        }
    }

    Enemy EnemyByName(string e)
    {
        return new Enemy();
    }

    void SpawnEnemy(string e, Vector3 p)
    {
        
    }
}
