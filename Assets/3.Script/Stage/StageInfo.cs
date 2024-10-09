using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [SerializeField]
    StageSO Data;

    StageData stage { get; set; }
    public Image stageimg;
    public Image[] Stars;
    Button inStage;

    private void Awake()
    {
        inStage = GetComponent<Button>();
        FirebaseManager.instance.GetStage(Data, (data) => 
        { 
            stage = data;
            inStage.onClick.AddListener(ChooseStage);
            if (data.isClear)
                stageimg.sprite = Resources.Load<Sprite>("Stage");
            for(int i = 0; i<3;i++)
            {
                if(data.Missions[i])
                  Stars[i].sprite = Resources.Load<Sprite>("Star");
            }
        });
    }

    void ChooseStage()
    {
        StartCoroutine(PanelManager.instance.ChangeAnimation("StageInfo"));
        StoryPanel.instance.setting(stage);
        StoryPanel.instance.startBtn.onClick.RemoveAllListeners();
        StoryPanel.instance.startBtn.onClick.AddListener(instage);
        StoryPanel.instance.startBtn.onClick.AddListener(() => BGMManager.instance.fadeout());
    }

    void instage()
    {
        PanelManager.instance.PanelOpen("Loading");
        Gamemanager.instance.CurrentStage = stage;
        UserData user = FirebaseManager.instance.userData;
        Gamemanager.instance.cookie = FirebaseManager.instance.cookieList.FindCookie(user.representCookie);
        LoadingManager.instance.currentScene = stage.Data.Stagename;
        FirebaseManager.instance.userData.heart -= Data.heart;
        LoadingManager.instance.SceneLoad(4);
        //로딩매니저 current scene에 data.name;
    }
}
