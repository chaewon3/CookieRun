using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class PlayPanel : MonoBehaviour
{
    public Button BackBtn;
    public Button homeBtn;
    public Button storyBtn;
    public Button RaidBtn;
    public CinemachineVirtualCamera stroyCam;
    public CinemachineVirtualCamera raidCam;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        storyBtn.onClick.AddListener(StoryBtnClick);
        RaidBtn.onClick.AddListener(RaidBtnClick);
        BackBtn.onClick.AddListener(maincam);
        homeBtn.onClick.AddListener(maincam);
    }
    private void OnEnable()
    {
        stroyCam.Priority = 11;
        raidCam.Priority = 12;
        UserData data = FirebaseManager.instance.userData;
        heartText.text = $"{data.heart}/150";
        coinText.text = data.coin.ToString("N0");
    }

    void StoryBtnClick()
    {
        raidCam.Priority = 9;
        stroyCam.Priority = 11;
        PanelManager.instance.PanelChange("Story");
    }

    void RaidBtnClick()
    {
        stroyCam.Priority = 9;
        raidCam.Priority = 11;
        PanelManager.instance.PanelChange("Raid");
    }

    void maincam()
    {
        stroyCam.Priority = 9;
        raidCam.Priority = 9;
    }
}
