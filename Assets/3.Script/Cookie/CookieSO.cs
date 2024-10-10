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
    Creamsoda,
    lemonZest,
    Chili
}

[CreateAssetMenu(fileName = "Cookie", menuName = "CookieRun/Add Cookie")]
public class CookieSO : ScriptableObject
{
    public Cookies cookie;
    public string name;
    public Grade grade;
    public Type type;
    public GameObject ModelPrefab;
    public GameObject LobbyPrefab;
    public Sprite portrait;
    public Sprite faceIcon;

    [Header("Info")]

    public float moveSpeed;
    public int baseHP;
    public int baseATK;
    public int baseDEF;

    public AudioClip Die;
    public AudioClip Win;
    public AudioClip Loose;

    [Header("Skills")]
    public string ATKName;
    [TextArea(3, 10)]
    public string ATKDes;
    public AudioClip ATKclip1;
    public AudioClip ATKclip2;
    public AudioClip ATKclip3;

    [Space(10)]
    public Sprite skillImg;
    public string skillName;
    public float skillCT;
    [TextArea(3, 10)]
    public string skillDes;
    public AudioClip skillclip1;
    public AudioClip skillclip2;
    public AudioClip skillclip3;

    [Space(10)]
    public Sprite ultimateImg;
    public string ultimateName;
    public float ultimateCT;
    [TextArea(3, 10)]
    public string ultimateDes;
    public AudioClip ultiamteclip;

    [Space(10)]
    public string dashName;
    public float dashCT;
    public string dashDes;
    public AudioClip dashclip;

    [Space(10)]
    public RuntimeAnimatorController ChairAnim;
}
