using System;
using System.Collections.Generic;

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

    public CookieList ()
    {
        //��ŰSO ��Ͽ��� BraveCookie�� ��Ű�� ������ �� ��Ű SO�� ������ ����, ��Ű ����Ʈ�� Add.
        //CookieData commoncookie = new CookieData();
        //cookies.Add();
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
