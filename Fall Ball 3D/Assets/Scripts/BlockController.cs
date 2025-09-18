using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    BlockController(bool death = true)
    {
        if (death)
        {
            StageSpawnner.instance.destroySpecific();
        }
        else
        {
            //sp[awnning squence

        }
    }
}
