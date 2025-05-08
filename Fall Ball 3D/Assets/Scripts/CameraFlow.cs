using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{

    public static CameraFlow instance = null;

    private void Awake()
    {
        instance = this;
    }

    public GameObject mainCam;
    void Start()
    {
        // Get the main camera and reference it as a GameObject
        mainCam = Camera.main.gameObject;
        Debug.Log("CamComponent: " +  mainCam.name);

        if(StageSpawnner.instance != null)
        {
            
        }
    }

    public void restructureCamPos(String message = null, bool editable = false)
    {
        if(message == "start")
        {
            this.transform.position = new Vector3(0, StageSpawnner.instance.blockCount * 2 + 15, 25);
            this.transform.eulerAngles = new Vector3(35, 180, 0);

            mainCam.transform.position = this.transform.position;
            mainCam.transform.eulerAngles = this.transform.eulerAngles;

            return;
        }

        if(!editable && message == null)
        {
            this.transform.position = new Vector3(0, this.transform.position.y - 2 , 25);
            mainCam.transform.position = this.transform.position;

        }
        else
        {
            // string parsing algo for boss battle or cam animations or something late game inclusions preparations
            return;
        }
    } 


    // calculations for cam position calculations

    /*Position: X: 0 Y: Dynamic + 15 Z: 25

    Rotation: X: 35 Y: 180 Z: 0*/




    /*    private void Update()
        {
            // Match position
            mainCam.transform.position = this.transform.position;

            // Match rotation
            mainCam.transform.rotation = this.transform.rotation;
        }*/
}
