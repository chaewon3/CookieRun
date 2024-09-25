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
    private GameObject changeEffect;

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
        changeEffect = GameObject.Find("SceneChange");
    }
    public void SceneLoad()
    {
        StartCoroutine(LoadGame(currentScene));
    }

    public void SceneChange()
    {
        StartCoroutine(Change());
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
        SceneChange();
    }        

    private IEnumerator Change()
    {
        if(changeEffect!= null)
           changeEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        asyncLoad.allowSceneActivation = true;
    }
}
