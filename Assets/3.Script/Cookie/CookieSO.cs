using UnityEngine;

public enum Grade
{
    COMMON,
    RARE,
    EPIC
}

[CreateAssetMenu(fileName = "Cookie", menuName = "Add Cookie")]
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
    [TextArea(5, 10)]
    public string ATKDes;

    [Space(10)]
    public float skillCT;
    [TextArea(5, 10)]
    public string skillDes;

    [Space(10)]
    public float ultimateCT;
    [TextArea(5, 10)]
    public string ultimateDes;

    [Space(10)]
    public float dashCT;
}
