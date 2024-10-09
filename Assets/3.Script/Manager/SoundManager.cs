using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource audioSource;

    public AudioClip btnclick;
    public AudioClip spawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void BtnClick()
    {
        audioSource.Stop();
        audioSource.clip = btnclick;
        audioSource.Play();
    }

    public void Spawn()
    {
        audioSource.Stop();
        audioSource.clip = btnclick;
        audioSource.Play();
    }

    public void clipPlay(AudioClip newclip)
    {
        audioSource.Stop();
        audioSource.clip = newclip;
        audioSource.Play();
    }

}
