using System;

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
public class Freind
{

}
