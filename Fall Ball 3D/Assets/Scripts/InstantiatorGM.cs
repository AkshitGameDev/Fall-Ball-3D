using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatorGM : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update
    void Awake()
    {
        var gameManager = Instantiate(GameManager);
        DontDestroyOnLoad(gameManager);
    }
}
