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
        if(stage == null)
        {
            stage = new StageData(Data);
        }
        inStage.onClick.AddListener(instage);
    }

    void instage()
    {
        Gamemanager.instance.CurrentStage = stage;
        //로딩매니저 current scene에 data.name;
    }
}
