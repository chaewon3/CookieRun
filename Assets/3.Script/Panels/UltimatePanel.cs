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
        //tofo : ���� ���ؼ� �߰��ؾ���. skillpanel�̶� ��� ��������
        Icon.sprite = data.portrait;
    }
}
