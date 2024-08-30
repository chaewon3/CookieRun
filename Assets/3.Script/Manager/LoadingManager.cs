using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance { get; private set; }

    public string currentScene = "LobbyScene";
    public GameObject touchBtn;

    private void Start()
    {
        StartCoroutine(LoadGame(currentScene));   
    }

    private IEnumerator LoadGame(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        float time = 0;
        while(!asyncLoad.isDone && time <= 2.5f)
        {
            // todo : ·Îµù¹Ù && time >=5f

            //if (asyncLoad.progress >= 0.9f && time >= 5f)
            //{
                
            //}
            time += Time.deltaTime;
            yield return null;
        }
        touchBtn.SetActive(true);
    }
}
