using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //splash
    public Animator splashAnim;
    private string animationStateName = "SplashAnimation";
    private bool hasFired = false;

    public static GameManager instance = null;

    public Canvas gamePlayCanvas;
    public GameObject[] Screans;

    string Level;
    int gems = 0;
    public Button TestBut;


    private void Awake()
    {
        instance = this;
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
        AnimatorStateInfo stateInfo = splashAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationStateName) && !hasFired)
        {
            OnAnimationComplete();
            hasFired = true; // Prevent multiple triggers
        }
    }

    void OnAnimationComplete()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchScreens(int screenName)
    {
        for (int i = 0; i < Screans.Length; i++)
        {
            if (screenName == i)
            {
                Screans[i].SetActive(true);
            }
            else
            {
                Screans[i].SetActive(false);
            }
        }
    }

    public void ConstructGameScene(){

    }
}
