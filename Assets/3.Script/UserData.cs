using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public enum Cookies
    {
        BraveCookie,
        Creamsoda,
        lemonZest
    }

    public string userid;
    public string username;
    public int level;
    public int exp;
    public int heart;
    public int coin;
    public Cookies representCookie; //¥Î«•ƒÌ≈∞

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
