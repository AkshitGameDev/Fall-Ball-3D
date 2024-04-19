using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScript : MonoBehaviour
{
    public void loadLevel()
    {
        GameManager.instance.OnAnimationComplete();
    }
}
