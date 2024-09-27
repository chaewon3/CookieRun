using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public List<CookieSO> cookieList = new List<CookieSO>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public CookieSO GetCookieSO(Cookies cookietype)
    {
        CookieSO SO = cookieList.Find(cookie => cookie.cookie == cookietype);
        return SO;
    }    
}

[Serializable]
public class UserData
{
    public string userid;
    public string username;
    public int level;
    public int exp;
    public int heart;
    public int coin;
    public Cookies representCookie; //��ǥ��Ű

    public UserData() { }

    public UserData(string userid)
    {
        this.userid = userid;
        username = "";
        level = 1;
        heart = 100;
        coin = 0;
        exp = 0;
        representCookie = Cookies.BraveCookie;
    }
}

[Serializable]
public class CookieList
{
    public List<CookieData> cookies = new List<CookieData>();

    public CookieList (Cookies cookie)
    {
        //��ŰSO ��Ͽ��� BraveCookie�� ��Ű�� ������ �� ��Ű SO�� ������ ����, ��Ű ����Ʈ�� Add.
        if (DataManager.instance == null)
            Debug.Log("�־���");
        CookieData commoncookie = new CookieData(DataManager.instance.GetCookieSO(cookie));
        cookies.Add(commoncookie);
    }

    public void AddCookie(CookieData data)
    {
        CookieData cookie = new CookieData(DataManager.instance.GetCookieSO(data.cookie), data);
        cookies.Add(data);
    }
}

[Serializable]
public class Inventory
{

}

[Serializable]
public class FriendData
{
    public List<string> friends;
    public List<string> friendRequest;

    public FriendData()
    {
        friends = new List<string>();
        friendRequest = new List<string>();
    }

}
