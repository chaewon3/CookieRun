using UnityEngine;

public enum Grade
{
    COMMON,
    RARE,
    EPIC
}

public enum Type
{
    Fire,
    Water,
    Light,
    Dark,
    Wind,
    Ground
}

public enum Cookies
{
    BraveCookie,
    Creamsoda,
    lemonZest
}

[CreateAssetMenu(fileName = "Cookie", menuName = "CookieRun/Add Cookie")]
public class CookieSO : ScriptableObject
{
    public Cookies cookie;
    public string name;
    public Grade grade;
    public Type type;
    public GameObject ModelPrefab;
    public Sprite portrait;

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
