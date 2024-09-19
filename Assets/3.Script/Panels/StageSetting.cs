using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StageSetting : MonoBehaviour
{
    public static StageSetting instance;

    public TextMeshProUGUI stagename;
    public TextMeshProUGUI mission1;
    public TextMeshProUGUI mission2;
    public TextMeshProUGUI mission3;
    public TextMeshProUGUI heart;
    public Button startBtn; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        this.gameObject.SetActive(false);
       
    }
    public void setting(StageData data)
    {
        stagename.text = data.Data.Stagename;
        mission1.text = data.Data.Mission_1Text;
        mission2.text = data.Data.Mission_2Text;
        mission3.text = data.Data.Mission_3Text;
        heart.text = data.Data.heart.ToString();
    }
}
