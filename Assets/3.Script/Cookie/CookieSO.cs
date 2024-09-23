using UnityEngine;

public enum Grade
{
    COMMON,
    RARE,
    EPIC
}

[CreateAssetMenu(fileName = "Cookie", menuName = "CookieRun/Add Cookie")]
public class CookieSO : ScriptableObject
{
    public int tableId;
    public string name;
    public Grade grade;
    public GameObject ModelPrefab;
    public Sprite icon;

    [Header("Info")]

    public float moveSpeed;
    public int baseHP;
    public int baseATK;
    public int baseDEF;

    [Header("Skills")]
    public string ATKName;
    [TextArea(3, 10)]
    public string ATKDes;

    [Space(10)]
    public string skillName;
    public float skillCT;
    [TextArea(3, 10)]
    public string skillDes;

    [Space(10)]
    public string ultimateName;
    public float ultimateCT;
    [TextArea(3, 10)]
    public string ultimateDes;

    [Space(10)]
    public string dashName;
    public float dashCT;
    public string dashDes;

    [Space(10)]
    public RuntimeAnimatorController ChairAnim;
}
