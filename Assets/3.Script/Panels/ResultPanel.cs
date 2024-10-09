using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public GameObject CapturePL;
    public Sprite Starimg;

    public AudioClip Winclip;
    public AudioClip Looseclip;
    public Button restartBtn;
    public Button exitBtn;
    public TextMeshProUGUI timeText;
    public GameObject[] missions = new GameObject[3];

    public TextMeshProUGUI cointext;
    public TextMeshProUGUI exptext;

    public GameObject Fail;
    public GameObject Win;
    public Image BG;
    public Color color;

    StageData data;
    Animator cookieanim;

    private void Awake()
    {
        cookieanim = CapturePL.GetComponentInChildren<Animator>();
        restartBtn.onClick.AddListener(Restart);
        exitBtn.onClick.AddListener(ExitGame);
        data = Stagemanager.instance.stagedata;
        Animator animator = GetComponent<Animator>();
        if (Stagemanager.instance.ClearGame)
        {
            animator.SetTrigger("Win");
            Win.SetActive(true);
        }
        else
        {
            animator.SetTrigger("Loose");
            cookieanim.SetTrigger("Loose");
            BG.color = color;
            //Fail.SetActive(true);
        }
    }
    private void Start()
    {
        float time = Stagemanager.instance.maxTime - Stagemanager.instance.time;
        timeText.text = $"{(int)time / 60}:{(time % 60).ToString("F2")}";

        if (Stagemanager.instance.ClearGame)
        {
            cookieanim.SetTrigger("Win");
            SoundManager.instance.clipPlay(Winclip);
        }
        else
        {
            cookieanim.SetTrigger("Loose");
            SoundManager.instance.clipPlay(Looseclip);
            return;
        }


        for (int i = 0; i < 3; i++) 
        {
            missions[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Stagemanager.instance.missionsText[i];
            if (Stagemanager.instance.ExecuteMission(i))
            {
                missions[i].transform.Find("star").GetComponent<Image>().sprite = Starimg;
            }
        }

        int coin = Stagemanager.instance.Coins;
        int exp = data.Data.exp;

        cointext.text = coin.ToString("N0");
        exptext.text = exp.ToString();
        FirebaseManager.instance.userData.coin += coin;
        FirebaseManager.instance.userData.GetExp(exp);
        FirebaseManager.instance.UpdateUserData();

        data.isClear = true;
        FirebaseManager.instance.UpdateStage(data);
    }

    public void Restart()
    {
        FirebaseManager.instance.userData.heart -= data.Data.heart;
        LoadingManager.instance.SceneLoad();
    }

    public void ExitGame()
    {
        LoadingManager.instance.currentScene = "LobbyScene";
        LoadingManager.instance.SceneLoad();
    }
}
