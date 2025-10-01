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
        mainCam = Camera.main.gameObject;

        if(StageSpawnner.instance != null)
        {
            
        }
    }


    public void restructureCamPos(String message = null, bool editable = false, float ypos = 0.0f)
    {
       
        if(message == "start")
        {
            this.transform.position = new Vector3(0, StageSpawnner.instance.blockCount * 2 + 15, 25);
            this.transform.eulerAngles = new Vector3(35, 180, 0);

            mainCam.transform.position = this.transform.position;
            mainCam.transform.eulerAngles = this.transform.eulerAngles;

            return;
        }

        
        if(message == "move") {
            this.transform.position = new Vector3(0, ypos + 15, 25);
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
            return;
        }
    } 


    /*Position: X: 0 Y: Dynamic + 15 Z: 25

    Rotation: X: 35 Y: 180 Z: 0*/




    /*    private void Update()
        {
            mainCam.transform.position = this.transform.position;

            // Match rotation
            mainCam.transform.rotation = this.transform.rotation;
        }*/
}
