using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public AudioSource audioSource;

    public AudioClip lobbyBGM;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LobbyBGM()
    {
        if (audioSource == null) return;
        audioSource.Stop();
        audioSource.clip = lobbyBGM;
        audioSource.Play();
    }
    
    public void BGMchange(AudioClip newclip)
    {
        audioSource.Stop();
        audioSource.clip = newclip;
        audioSource.Play();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
    }

    public void fadeout()
    {
        StartCoroutine(fadeoutCoroutine());
    }
    IEnumerator fadeoutCoroutine()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < 2; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / 2);
            yield return null;
        }

        audioSource.volume = 0; // 최종 볼륨을 0으로 설정
        audioSource.Stop(); // 음악 정지
    }
}
