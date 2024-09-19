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
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(currentScene);
        asyncLoad.allowSceneActivation = false;

        float time = 0;
        while (!asyncLoad.isDone && time <= 2.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        touchBtn.SetActive(true);
    }

    private void OnLevelWasLoaded(int level)
    {
        SceneChange = GameObject.Find("SceneChange");
    }

    public void SceneLoading()
    {
        StartCoroutine(LoadGame(currentScene));
    }

    private IEnumerator LoadGame(string scene)
    {
        PanelManager.instance.PanelChange("Loading");
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        float time = 0;
        while(!asyncLoad.isDone && time <= 2.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Change());
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
