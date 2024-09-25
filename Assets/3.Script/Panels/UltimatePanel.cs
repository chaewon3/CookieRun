using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimatePanel : MonoBehaviour
{
    public Image Icon;
    public Color color;


    public void SettingUltimatePanel(CookieSO data)
    {
        //tofo : 색다 정해서 추가해야함. skillpanel이랑 묶어도 괜찮을듯
        Icon.sprite = data.portrait;
    }
}
