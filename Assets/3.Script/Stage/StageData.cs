using UnityEngine;

[System.Serializable]
public class StageData
{
    public StageSO Data { get; set; }
    public bool isClear;
    public bool[] Jellies;
    public bool[] Missions = new bool[3];

    public StageData(StageSO data)
    {
        Data = data;
        isClear = false;
        Jellies = new bool[data.Jellies];
    }
}
