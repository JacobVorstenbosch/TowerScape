using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONManager : MonoBehaviour {

    [System.Serializable]
    public struct EncounterJSON
    {
        public string[] enemies;
        public int[] counts;
    }

    [System.Serializable]
    public struct EnemyJSON
    {
        public string type;    //used for spawning
        public int etype;  //enemy type used
        public float hp;       //max health
        public float dmg;      //damage per swing
        public float atsp;     //swings per second
        public float radius;   //used for spawning, allows for custom packing
    }

    public TextAsset encounterText;
    public TextAsset enemyText;

    public int currentFloor;

    private List<EncounterJSON> m_encounterList;
    private Dictionary<string, EnemyJSON> m_enemyDict;

	// Use this for initialization
	void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

        //fill jsons
        m_encounterList = new List<EncounterJSON>();
        PrepEncounters();

        m_enemyDict = new Dictionary<string, EnemyJSON>();
        PrepEnemies();
        currentFloor = 0;
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public EncounterJSON GetEncounterByFloor(int floorNumber)
    {
        return m_encounterList[floorNumber];
    }

    public EnemyJSON GetEnemyByName(string name)
    {
        return m_enemyDict[name];
    }

    void PrepEncounters()
    {
        string encounters = encounterText.ToString();
        int start = 0;
        int end = 0;
        while (true)
        {
            start = encounters.IndexOf('{', end);
            if (start < 0)
                break;
            end = encounters.IndexOf('}', start);
            end++;
            m_encounterList.Add(JsonUtility.FromJson<EncounterJSON>(encounters.Substring(start, end - start)));
        }
    }

    void PrepEnemies()
    {
        string enemies = enemyText.ToString();
        int start = 0;
        int end = 0;
        while (end < enemies.Length)
        {
            start = enemies.IndexOf('{', end);
            if (start < 0)
                break;
            end = enemies.IndexOf('}', start);
            end++;
            EnemyJSON e = JsonUtility.FromJson<EnemyJSON>(enemies.Substring(start, end - start));
            m_enemyDict.Add(e.type, e);
        }
    }

}
