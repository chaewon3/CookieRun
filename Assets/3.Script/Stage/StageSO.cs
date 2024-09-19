using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mission
{
    Clear,
    NoHit,
    ClearTime,
    AllTorch
}


[CreateAssetMenu(fileName = "Stage",menuName ="CookieRun/Add Stage")]
public class StageSO : ScriptableObject
{
    public string Stagename;
    public int Jellies;
    public float time;
    public float clearTime;
    public int heart;

    public Mission Mission_1;
    [TextArea(2, 10)]
    public string Mission_1Text;

    public Mission Mission_2;
    [TextArea(2, 10)]
    public string Mission_2Text;

    public Mission Mission_3;
    [TextArea(2, 10)]
    public string Mission_3Text;

}
