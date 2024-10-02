using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RPCHpBar : MonoBehaviourPunCallbacks
{
    public Transform Player;

    public TextMeshProUGUI HPtext;
    public TextMeshProUGUI PlNumber;
    public Slider HPbar;

    public Player player;
    Camera MainCam;

    private void Start()
    {
        MainCam = Camera.main;
        int number = 1; // �÷��̾� �ѹ����� ����
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player == this.player)
            {

                PlNumber.text = $"{number}P";
            }
            number++;
        }
    }

    private void LateUpdate()
    {
        transform.position = MainCam.WorldToScreenPoint(Player.position + new Vector3(0, 3f, 0));
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != player) return;
        if(changedProps.ContainsKey("HP"))
        {
            int hp = (int)changedProps["HP"];
            float hpPer = (float)changedProps["HPPer"];
            HPtext.text = hp.ToString();
            HPbar.value = hpPer;
        }
    }
}
