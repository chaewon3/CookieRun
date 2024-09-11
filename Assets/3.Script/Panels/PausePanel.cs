using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Button restartBtn;

    private void Awake()
    {
        restartBtn.onClick.AddListener(restartClick);
    }

    void restartClick()
    {
        StartCoroutine(canvasOff());
    }

    public void openPanel()
    {
        gameObject.SetActive(true);
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
