using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RPCHpBar : MonoBehaviourPunCallbacks
{
    public Transform playerTransform;

    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI PlNumber;
    public Slider HPbar;
    public Image slider;
    public Sprite greenSlider;

    public Player player;
    Camera MainCam;

    private void Start()
    {
        MainCam = Camera.main;
        int number = 1; // 플레이어 넘버링을 위해
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player == this.player)
            {
                PlNumber.text = $"{number}P";
                if (player == PhotonNetwork.LocalPlayer)
                    slider.sprite = greenSlider;
            }
            number++;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) Destroy(gameObject);
        transform.position = MainCam.WorldToScreenPoint(playerTransform.position + new Vector3(0, 3f, 0));
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != player) return;
        if(changedProps.ContainsKey("HP"))
        {
            int hp = (int)changedProps["HP"];
            HPtext.text = hp.ToString();
        }
        if(changedProps.ContainsKey("HPPer"))
        {
            float hpPer = (float)changedProps["HPPer"];
            HPbar.value = hpPer;
        }
    }
}
