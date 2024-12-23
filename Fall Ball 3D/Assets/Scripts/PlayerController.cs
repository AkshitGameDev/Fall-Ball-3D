using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField]
    Camera camera = null;
    GameObject cameraPoint = null;
    [SerializeField]
    float bounceMagnitude = 20.0f;
    [SerializeField]
    float jumpHight = 1.7f;
    bool dirUp = false;
    float bounce = 0.0f;

    bool holding = false;

    GameObject PlayerPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        PlayerPoint = StageSpawnner.instance.playerPoint;
    }

    void bounceBack()
    {
        dirUp = false;
    }

    void checkHold()
    {

    }

    // Update is called once per frame
    void Update()
    {
       checkHold();
       if (!GameManager.instance.isPlaying) return;
       if(!holding) {//defalt 
            if (dirUp)
            {
                bounce += (this.jumpHight * Time.deltaTime);
            }
            else
            {
                bounce -= (this.jumpHight * Time.deltaTime);
            }

            if(bounce >= 1)
            {
                bounce = 1;
                dirUp = false;
            }
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
           this.gameObject.transform.position.y + Mathf.Sin(bounce),
           this.gameObject.transform.position.z);
        }

       
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "breakable" && !holding)
        {
            Debug.Log("setting dir up true");
            dirUp = true;
        }
    }

    
}
