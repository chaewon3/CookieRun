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
}

[Serializable]
public class CookieList
{
    public List<CookieData> cookies = new List<CookieData>();

    public CookieList ()
    {
        //쿠키SO 목록에서 BraveCookie인 쿠키를 가져와 그 쿠키 SO로 데이터 생성, 쿠키 리스트에 Add.
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
