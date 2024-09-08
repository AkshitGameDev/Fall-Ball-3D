using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawnner : MonoBehaviour
{

    public static StageSpawnner instance;

    public int TotalblocksSpawnnedInTower = 0;

    public GameObject[] blocksList;

    [SerializeField]
    GameObject stagePrefab = null;
    [SerializeField]
    GameObject blockPrefab = null;

    GameObject stageSpawnned;
    GameObject BlockParent;

    int spawnned = 0;




    // Start is called before the first frame updat
    void Awake()
    {
        instance = this;
    }

    public void getCredentials(int blocks)
    {
        Debug.Log("number of blocks" + blocks);
        TotalblocksSpawnnedInTower = blocks;
        SpawnStage();

        for (int i = 0; i < TotalblocksSpawnnedInTower; i++) { 
        if(i == 0)
            {
                SpawnInitialBlock();
            }
        else
            {
                SpawnTowerBlocks();
            }
        }


    }

    void SpawnStage ()
    { 
        var spawn = Instantiate(stagePrefab, new Vector3(0,0,0), new Quaternion().normalized);
        stageSpawnned = spawn;
        BlockParent = spawn.transform.GetChild(0).gameObject;

    }

    void SpawnInitialBlock()
    {
        var s_block = Instantiate(blockPrefab, new Vector3(0, 0, 0), BlockParent.transform.rotation);
        s_block.transform.SetParent(BlockParent.transform, false);
    }


    void SpawnTowerBlocks()
    {
        spawnned += 2;
        var s_block = Instantiate(blockPrefab, new Vector3( BlockParent.transform.position.x, spawnned, BlockParent.transform.position.z), BlockParent.transform.rotation);
        s_block.transform.SetParent(BlockParent.transform, false);
        Debug.Log("spawnned value" + spawnned);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
