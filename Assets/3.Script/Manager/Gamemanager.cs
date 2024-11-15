using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; set; }

    public bool canMove { get; set; } = false;
    public bool OnGame { get; set; } = true; // todo : 나중에 false로 시작해야함
    public bool IsDie { get; set; } = false;
    public StageData CurrentStage { get; set; }
    public CookieData cookie { get; set; }
    //public CookieData[] cookies = new CookieData[3];
    public List<Player> players;
    public Dictionary<int, UserData> playersData = new Dictionary<int, UserData>();
    public Dictionary<int, CookieData> playersCookie = new Dictionary<int, CookieData>();

    public string errorMsg = string.Empty;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
