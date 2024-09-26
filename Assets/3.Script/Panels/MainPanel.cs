using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPanel : MonoBehaviour
{
    UserData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI heartText;
    public TextMeshProUGUI coinText;    

    private void OnEnable()
    {
        data = FirebaseManager.instance.userData;
        nameText.text = data.username;
        levelText.text = data.level.ToString();
        heartText.text = $"{data.heart}/150";
        coinText.text = data.coin.ToString();
    }
}