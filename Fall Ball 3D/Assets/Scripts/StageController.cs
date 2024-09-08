using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public GameObject pipe = null;
    public GameObject BlockParent;

    public float RotationSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        BlockParent.transform.Rotate(BlockParent.transform.rotation.x, BlockParent.transform.rotation.y + RotationSpeed, BlockParent.transform.rotation.z);
    }
}
