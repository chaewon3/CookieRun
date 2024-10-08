using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// ����� ������Ŭ����
/// </summary>
[System.Serializable]
public class CookieData
{
    [JsonIgnore]
    private CookieSO data;
    [JsonIgnore]
    public CookieSO Data { get => data; set => data = value; }
    public Cookies cookie; // cookie�� key�� cookieSO�� �ҷ��� ��
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
    { // todo : ������ ������ ��ư���� �� ��
        currentlevel++;

        int bonus = currentlevel / 20 + 1;
        HP += 41 * bonus;
        ATK += 5 * bonus;
        DEF += 5 * bonus;
    }
}
