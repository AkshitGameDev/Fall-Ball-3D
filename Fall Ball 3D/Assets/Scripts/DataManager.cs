using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager Instance = null;



    public TextAsset levelJSON;

    

    [System.Serializable]
    public class Level
    {
        public int level;
        public int blocks;
    }

    [System.Serializable]
    public class LevelList
    {
        public Level[] levels;
    }

    public LevelList myLevelList = new LevelList();
    void Awake()
    {
        Instance = this;
        Debug.Log("level text" + levelJSON.text);
        myLevelList = JsonUtility.FromJson<LevelList>(levelJSON.text);
        Debug.Log("Level list" + myLevelList.levels[1].blocks);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
