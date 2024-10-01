using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PLHPBar : MonoBehaviour
{
    public Transform Player;

    public TextMeshProUGUI HPtext;
    public Slider HPbar;

    ICookie cookie;

    Camera MainCam;

    private void Start()
    {
        Player = FindObjectOfType<PlayerMove>().gameObject.transform;

        MainCam = Camera.main;
        cookie = Player.GetComponent<PlayerMove>().Cookie;
    }

    private void LateUpdate()
    {
        transform.position = MainCam.WorldToScreenPoint(Player.position + new Vector3(0, 3f, 0));
        HPtext.text = cookie.CurrentHP.ToString();
        HPbar.value = cookie.HPPer;
    }
}
