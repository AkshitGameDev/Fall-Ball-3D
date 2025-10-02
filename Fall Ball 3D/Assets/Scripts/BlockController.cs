using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    //checking wether the block is the bottommost block of the queue 
    bool bottomMostBlock = false;

    public void Settings( bool _bottomMostBlock)
    {
        bottomMostBlock = _bottomMostBlock;
    }

    private void OnDestroy()
    {
        if(bottomMostBlock)
        {
            Debug.Log("I am ded but dont wory i shall meet you in valhalla ");
        }
    }
}
