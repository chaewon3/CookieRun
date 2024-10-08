using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Trophy : MonoBehaviour
{
    public Image trophy;
    public Image icon;
    public TextMeshProUGUI name;

    public Image goldTrophy;
    public Image silverTrophy;
    public Image bronzeTrophy;

    public Sprite DamageIcon;
    public Sprite LiveIcon;


    public void GetTrophy(Sprite trophy, Sprite icon, string name)
    {
        this.trophy.sprite = trophy;
        this.icon.sprite = icon;
        this.name.text = name;
        this.icon.gameObject.SetActive(true);
        this.name.gameObject.SetActive(true);
    }
}
