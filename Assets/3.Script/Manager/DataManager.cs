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
    public Cookies representCookie; //대표쿠키

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

    public void GetExp(int exp)
    {
        exp += exp;
        int exprequest = 200 + level * 100;
        while(exp >= exprequest)
        {
            level++;
            exp -= exprequest;
        }
    }

    public float ExpPer()
    {
        int maxexp = 200 + level * 100;
        return exp / maxexp;
    }
}

[Serializable]
public class CookieList
{
    public List<CookieData> cookies = new List<CookieData>();

    public CookieList() { }
    public CookieList (Cookies cookie)
    {
        //쿠키SO 목록에서 BraveCookie인 쿠키를 가져와 그 쿠키 SO로 데이터 생성, 쿠키 리스트에 Add.
        if (DataManager.instance == null)
            Debug.Log("왜없지");
        CookieData commoncookie = new CookieData(DataManager.instance.GetCookieSO(cookie));
        cookies.Add(commoncookie);
    }

    public void FindCookieSO(CookieData data)
    {
        CookieData cookie = new CookieData(DataManager.instance.GetCookieSO(data.cookie), data);
        cookies.Add(cookie);
    }

    public CookieData FindCookie(Cookies type)
    {
        foreach(CookieData cookie in cookies)
        {
            if (cookie.cookie == type)
                return cookie;
        }
        return null;
    }

    public void AddCookie(Cookies type)
    {
        CookieData cookie = new CookieData(DataManager.instance.GetCookieSO(type));
        cookies.Add(cookie);
    }

    public void UpdateCookie(CookieData data)
    {
        for (int i = 0; i < cookies.Count; i++)
        {
            if (cookies[i].cookie == data.cookie)
            {
                cookies[i] = data; 
                break; 
            }
        }
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
