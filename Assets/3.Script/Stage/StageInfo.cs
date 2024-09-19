using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    [SerializeField]
    StageSO Data;

    StageData stage { get; set; }

    Button inStage;

    private void Awake()
    {
        inStage = GetComponent<Button>(); 
    }
    private void Start()
    {
        FirebaseManager.instance.GetStage(Data, (data) => stage = data);
        inStage.onClick.AddListener(ChooseStage);
    }

    void ChooseStage()
    {
        StartCoroutine(PanelManager.instance.ChangeAnimation("StageInfo"));

        StageSetting.instance.setting(stage);
        StageSetting.instance.startBtn.onClick.RemoveAllListeners();
        StageSetting.instance.startBtn.onClick.AddListener(instage);
    }

    void instage()
    {
        PanelManager.instance.PanelOpen("Loading");
        Gamemanager.instance.CurrentStage = stage;
        print($"stage : {Gamemanager.instance.CurrentStage.Data.Stagename}");
        LoadingManager.instance.currentScene = stage.Data.Stagename;
        LoadingManager.instance.SceneLoading();
        //로딩매니저 current scene에 data.name;
    }
}
