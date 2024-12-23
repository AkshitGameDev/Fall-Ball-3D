using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject pipe = null;
    public GameObject BlockParent;



    public float RotationSpeed = 2f;



   void DataInit(object data)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        RotationSpeed += 0.1f;
        BlockParent.transform.Rotate(BlockParent.transform.rotation.x,RotationSpeed, BlockParent.transform.rotation.z);

    }
}
