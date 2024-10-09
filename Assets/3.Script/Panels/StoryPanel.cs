using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class StoryPanel : MonoBehaviour
{
    public static StoryPanel instance;

    public AudioClip bgmclip;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI coinText;
    public Button homeBtn;
    public CinemachineVirtualCamera storyCam;
    public Sprite star;

    public TextMeshProUGUI stagename;
    public TextMeshProUGUI mission1;
    public TextMeshProUGUI mission2;
    public TextMeshProUGUI mission3;
    public Image[] stars = new Image[3];

    public TextMeshProUGUI heart;
    public Button startBtn;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        homeBtn.onClick.AddListener(() => storyCam.Priority = 9);
    }

    private void OnEnable()
    {
        UserData data = FirebaseManager.instance.userData;
        heartText.text = $"{data.heart}/150";
        coinText.text = data.coin.ToString("N0");
        BGMManager.instance.BGMchange(bgmclip);
    }

    private void OnDisable()
    {
        BGMManager.instance.LobbyBGM();
    }

    public void setting(StageData data)
    {
        stagename.text = data.Data.Stagename;
        mission1.text = data.Data.Mission_1Text;
        mission2.text = data.Data.Mission_2Text;
        mission3.text = data.Data.Mission_3Text;
        heart.text = data.Data.heart.ToString();

        for(int i = 0;i<3;i++)
        {
            if (data.Missions[i])
                stars[i].sprite = star;
        }
    }

}
