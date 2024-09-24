using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; set; }

    public StageData CurrentStage { get; set; }
    public CookieData[] cookies = new CookieData[3];
    public Dictionary<int, UserData> players = new Dictionary<int, UserData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
