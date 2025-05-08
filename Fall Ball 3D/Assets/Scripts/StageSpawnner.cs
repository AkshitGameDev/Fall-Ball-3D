using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageSpawnner : MonoBehaviour
{

    public static StageSpawnner instance;

    public int TotalblocksSpawnnedInTower = 0;

    public List<GameObject> blocksList;

    [SerializeField]
    GameObject stagePrefab = null;
    [SerializeField]
    GameObject blockPrefab = null;
    [SerializeField]
    GameObject PlayerPrefab = null;

    GameObject stageSpawnned;
    GameObject BlockParent;
    [DoNotSerialize]
    public GameObject playerPoint;


    GameObject PlayerObject = null;

    int spawnned = 0;

    bool spawnPlayer = false;
    [DoNotSerialize]
    public int blockCount = 0;


    // Start is called before the first frame updat
    void Awake()
    {
        instance = this;
    }

    public void getCredentials(int blocks)
    {
        blockCount = blocks;
        Debug.Log("Block Count: " + blockCount);
        CameraFlow.instance.restructureCamPos("start");
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
                if (blocksList.Count == TotalblocksSpawnnedInTower - 1) spawnPlayer = true;
                SpawnTowerBlocks();
            }

        }

     

    }

    void SpawnStage ()
    { 
        var spawn = Instantiate(stagePrefab, new Vector3(0,0,0), new Quaternion().normalized);
        stageSpawnned = spawn;
        BlockParent = spawn.transform.GetChild(0).gameObject;
        playerPoint = spawn.transform.GetChild(2).gameObject;
    }

    void SpawnInitialBlock()
    {
        var s_block = Instantiate(blockPrefab, new Vector3(0, 0, 0), BlockParent.transform.rotation);
        s_block.transform.SetParent(BlockParent.transform, false);
        playerPoint.transform.localPosition = new Vector3(playerPoint.transform.localPosition.x, playerPoint.transform.localPosition.y + 2, playerPoint.transform.localPosition.z);
        blocksList.Add(s_block);
    
    }


    void SpawnTowerBlocks()
    {
        spawnned += 2;
        var s_block = Instantiate(blockPrefab, new Vector3( BlockParent.transform.position.x, spawnned, BlockParent.transform.position.z), BlockParent.transform.rotation);
        s_block.transform.SetParent(BlockParent.transform, false);
        // Debug.Log("spawnned value" + spawnned);
        playerPoint.transform.localPosition = new Vector3(playerPoint.transform.localPosition.x, playerPoint.transform.localPosition.y + 2, playerPoint.transform.localPosition.z);
        blocksList.Add(s_block);
        if (spawnPlayer)
        {
            Debug.Log("spawnning Player");
            var player = Instantiate(PlayerPrefab, playerPoint.transform.position, new Quaternion().normalized);
            PlayerObject = player;

            spawnPlayer = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

