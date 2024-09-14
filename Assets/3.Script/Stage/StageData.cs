using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public StageSO Data { get; set; }
    public bool isClear;
    public bool[] Jellies;
    public bool[] Missions = new bool[3];

    public StageData(StageSO data)
    {
        isClear = false;
        Jellies = new bool[data.Jellies];
    }
}
