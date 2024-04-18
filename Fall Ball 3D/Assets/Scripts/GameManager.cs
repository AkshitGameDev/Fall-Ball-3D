using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public Canvas gamePlayCanvas;
    public GameObject[] Screans;

    string Level;
    int gems = 0;
    public Button TestBut;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetString("CurLevel") == null)
        {
            PlayerPrefs.SetString("CurLevel", "0");
            PlayerPrefs.SetInt("CurGems", 50);
            Level = PlayerPrefs.GetString("CurLevel");
            gems = PlayerPrefs.GetInt("CurGems");
        }
        else{
            Level = PlayerPrefs.GetString("CurLevel");
            gems = PlayerPrefs.GetInt("CurGems");
        }

    }
    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ConstructGameScene(){

    }
}
