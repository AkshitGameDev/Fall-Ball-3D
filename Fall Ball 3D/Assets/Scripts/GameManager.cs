using System;
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

    public GameObject ButtonPlay = null;
    public GameObject ButtonSettings = null;
    public GameObject ButtonShop = null;

    //settings



    //shop



    //

    public static GameManager instance = null;




    public Canvas gamePlayCanvas;
    public GameObject[] Screans;
    AnimatorStateInfo stateInfo;

    string Level;
    int gems = 0;
    public Button TestButon;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ScreenIndex"></param>

    public void SwitchScreens(int ScreenIndex)
    {
        for (int i = 0; i < Screans.Length; i++)
        {
            if (ScreenIndex == i)
            {
                Screans[i].SetActive(true);
            }
            else
            {
                Screans[i].SetActive(false);
            }
        }
    }

    //On Screen Opening 
    public void CreateMenuScene()
    {
        AudioManager.instance.playLooping(AudioManager.instance.MenuSfx);
    }



    //On Screen Close/Pause 




    // Button Events 
    public void onClickPlayButton()
    {
        Debug.Log("click btn");
        SwitchScreens(2);
        AudioManager.instance.Tween(1);
    }

    public void ConstructGameScene(int level){

    }


    // PLay button Clicke Events

       
}
