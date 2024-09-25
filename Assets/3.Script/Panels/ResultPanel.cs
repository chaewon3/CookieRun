using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public GameObject CapturePL;
    public Sprite Starimg;

    public Button restartBtn;
    public TextMeshProUGUI timeText;
    public GameObject[] missions = new GameObject[3];

    Animator cookieanim;

    private void Awake()
    {
        cookieanim = CapturePL.GetComponentInChildren<Animator>();
        restartBtn.onClick.AddListener(Restart);
    }
    private void Start()
    {
        if(Stagemanager.instance.ClearGame)
           cookieanim.SetTrigger("Win");
        else
            cookieanim.SetTrigger("Loose");

        float time = Stagemanager.instance.maxTime-Stagemanager.instance.time;
        timeText.text = $"{(int)time / 60}:{(time % 60).ToString("F2")}";

        for (int i = 0; i < 3; i++) 
        {
            missions[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Stagemanager.instance.missionsText[i];
            if (Stagemanager.instance.ExecuteMission(i))
            {
                missions[i].transform.Find("star").GetComponent<Image>().sprite = Starimg;
            }
        }

        
    }

    public void Restart()
    {
        LoadingManager.instance.SceneLoad();
    }
}
