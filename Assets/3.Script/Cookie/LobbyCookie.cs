using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCookie : MonoBehaviour
{
    public CookieSO data;
    public void Win()
    {
        SoundManager.instance.clipPlay(data.Win);
    }
    public void Loose()
    {
        SoundManager.instance.clipPlay(data.Loose);
    }
}
