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
    private string animationStateName = "ending";
    private bool hasFired = false;

    //Menu
    public GameObject MinusPoint = null;
    public GameObject PlusPoint = null;


    public static GameManager instance = null;

    public Canvas gamePlayCanvas;
    public GameObject[] Screans;
    AnimatorStateInfo stateInfo;

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
    void Start(){
        stateInfo = splashAnim.GetCurrentAnimatorStateInfo(0);
    }
    public void OnAnimationComplete()
    {
        CreateMenuScene();
        SwitchScreens(1);
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
    public void CreateMenuScene()
    {
        AudioManager.instance.playLooping(AudioManager.instance.MenuSfx);
    }
    public void ConstructGameScene(int level){

    }
}
