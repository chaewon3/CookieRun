using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 저장용 데이터클래스
/// </summary>
[System.Serializable]
public class CookieData
{
    [JsonIgnore]
    private CookieSO data;
    [JsonIgnore]
    public CookieSO Data { get => data; set => data = value; }
    public Cookies cookie; // cookie를 key로 cookieSO를 불러올 것
    public int currentlevel;
    public int HP;
    public int ATK;
    public int DEF;

    public int[] equipment = new int[4];

    public CookieData() { }
    public CookieData(CookieSO data, int level = 1)
    {
        this.data = data;
        this.currentlevel = level;
        cookie = data.cookie;
        HP = data.baseHP;
        ATK = data.baseATK;
        DEF = data.baseDEF;
    }

    public CookieData(CookieSO SOdata, CookieData data) : this(SOdata, data.currentlevel)
    {
        this.equipment = data.equipment;
        this.HP = data.HP;
        this.ATK = data.ATK;
        this.DEF = data.DEF;
    }


    public void levelUP()
    { // todo : 레벨업 제한은 버튼에서 할 것
        currentlevel++;

        int bonus = currentlevel / 20 + 1;
        HP += 41 * bonus;
        ATK += 5 * bonus;
        DEF += 5 * bonus;
    }
}
