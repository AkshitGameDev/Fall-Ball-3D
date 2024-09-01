using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{



    public static AudioManager instance = null;
    AudioSource Source = null;
    bool mute = false;
    public Button muteBut = null;

    // public audio clips
    public AudioClip MenuSfx = null;
    public AudioClip startSfx = null;

    //Lean Tween variables
    public float twoWayTweenTime = 1f;
public float incDicMultiplier = 7;

    private void Awake()
    {
        instance = this;
        Source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    public void playSfx(AudioClip clip)
    {
        if (!mute)
        {
            Source.PlayOneShot(clip);
        }
    }
    public void playLooping(AudioClip clip) {
        if (!mute)
        {
            Source.loop = true;
            Source.PlayOneShot(clip);
        }
    }

    // direction can be 1 || 0 : 1 to decreas the volume and 0   for increasing the volume
    public void Tween(int direction)
    {
        if (direction == 1){
            Debug.Log("twinning audio down");
            Source.volume = Source.volume / incDicMultiplier;
            Debug.Log("twinning audio down2");
        }
        else if (direction == 0){

            Debug.Log("twinning audio UP");
            Source.volume =  Source.volume * incDicMultiplier;
            }
        else
            Debug.LogWarning("Invalid Direction /n direction can be 1 || 0 : 1 to decreas the volume and 0 for increasing the volume");
    }

}
