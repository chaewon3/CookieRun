using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance { get; private set; }


    private AsyncOperation asyncLoad;
    public string currentScene = "LobbyScene";
    public GameObject touchBtn;
    public GameObject SceneChange;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadGame(currentScene));   
    }

    private IEnumerator LoadGame(string scene)
    {
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        float time = 0;
        while(!asyncLoad.isDone && time <= 2.5f)
        {
            // todo : ·Îµù¹Ù 

            //if (asyncLoad.progress >= 0.9f && time >= 5f)
            //{
                
            //}
            time += Time.deltaTime;
            yield return null;
        }
        touchBtn.SetActive(true);
    }
        
    public void Scenechange()
    {
        StartCoroutine(Change());
    }

    public IEnumerator Change()
    {
        SceneChange.SetActive(true);
        yield return new WaitForSeconds(1f);
        asyncLoad.allowSceneActivation = true;
    }
}
