using UnityEngine;

/// <summary>
/// 저장용 데이터클래스
/// </summary>
[System.Serializable]
public class CookieData
{
    private CookieSO data;
    public CookieSO Data { get => data; set => data = value; }
    public int tableId;
    public int level;
    public int HP;
    public int ATK;
    public int DEF;

    public int[] equipment = new int[4];

    public CookieData(CookieSO data, int level = 1)
    {
        this.data = data;
        this.level = level;
        tableId = data.tableId;
        HP = data.baseHP;
        ATK = data.baseATK;
        DEF = data.baseDEF;
    }

    // 안써도 될거같기도? 나중에 저장할때 테스트해보고 지우기
    public CookieData(CookieSO data, int level, int HP, int ATK, int DEF, int[] equipment) : this(data, level)
    {
        this.equipment = equipment;
        this.HP = HP;
        this.ATK = ATK;
        this.DEF = DEF;
    }

    public void levelUP()
    { // todo : 레벨업 제한은 버튼에서 할 것
        level++;

        int bonus = level / 20 + 1;
        HP += 41 * bonus;
        ATK += 5 * bonus;
        DEF += 5 * bonus;
    }
}
