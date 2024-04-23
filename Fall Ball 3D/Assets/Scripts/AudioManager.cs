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
}
