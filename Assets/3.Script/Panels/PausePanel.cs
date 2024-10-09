using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausePanel : MonoBehaviour
{
    public Button restartBtn;
    public Button stopBtn;
    public GameObject resultpanel;

    public Sprite Starimg;
    public GameObject[] missions = new GameObject[3];


    private void Awake()
    {
        restartBtn.onClick.AddListener(restartClick);
        stopBtn.onClick.AddListener(StopClick);
    }

    void restartClick()
    {
        StartCoroutine(canvasOff());
    }
    void StopClick()
    {
        StartCoroutine(canvasOff());
        resultpanel.SetActive(true);
    }

    public void openPanel()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            missions[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Stagemanager.instance.missionsText[i];
            if (Stagemanager.instance.ExecuteMission(i))
            {
                missions[i].transform.Find("star").GetComponent<Image>().sprite = Starimg;
            }
        }

        Time.timeScale = 0;
        StartCoroutine(canvasAlpha());
    }

    IEnumerator canvasAlpha()
    {
        float starttime = 0;

        while (starttime < 0.3f)
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, starttime / 0.3f);
            starttime += Time.unscaledDeltaTime;
            yield return null;
        }

        
        GetComponent<CanvasGroup>().alpha = 1;
    }

    IEnumerator canvasOff()
    {
        float starttime = 0;

        while (starttime < 0.3f)
        {
            GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, starttime / 0.3f);
            starttime += Time.unscaledDeltaTime;
            yield return null;
        }
        GetComponent<CanvasGroup>().alpha = 0;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    
}
