using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawnner : MonoBehaviour
{

    public static StageSpawnner instance;

    public int TotalblocksSpawnnedInTower = 0;

    public GameObject[] blocksList;


    // Start is called before the first frame updat
    void Awake()
    {
        instance = this;
    }

    public void getCredentials(int blocks)
    {
        Debug.Log("number of blocks" + blocks);
        TotalblocksSpawnnedInTower = blocks;



    }

    void SpawnInitialBlock()
    {

    }


    void SpawnTowerBlocks()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
