using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using Cinemachine;

public class RaidManager : MonoBehaviour
{
    public RaidManager instance { get; set; }

    public GameObject[] PlayerPositions;
    public InputActionAsset playerMove;
    public CinemachineVirtualCamera camera;

    public Dictionary<int, UserData> players { get; private set; } = new Dictionary<int, UserData>();
    public Dictionary<int, CookieData> playersCookie { get; private set; } = new Dictionary<int, CookieData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        players = Gamemanager.instance.playersData;
        playersCookie = Gamemanager.instance.playersCookie;

    }

    private void Start()
    {
        int playernum = 0;
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            CookieData data = playersCookie[player.ActorNumber];
            if (player == PhotonNetwork.LocalPlayer)
            {
                GameObject localPL = PhotonNetwork.Instantiate("LocalPlayer", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation);
                var playerInput = localPL.AddComponent<PlayerInput>();
                playerInput.actions = playerMove;
                playerInput.defaultControlScheme = "PC";
                playerInput.defaultActionMap = "PlayerAction";
                playerInput.ActivateInput();
                localPL.AddComponent<PlayerMove>();

                CookieBase localCK = PhotonNetwork.Instantiate($"{data.cookie.ToString()}", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation).GetComponent<CookieBase>();
                localCK.Cookie = data;
                camera.Follow = PlayerPositions[playernum].transform;

                int parentid = localPL.GetComponent<PhotonView>().ViewID;
                int childid = localCK.gameObject.GetComponent<PhotonView>().ViewID;
                PhotonView photonView = localPL.GetComponent<PhotonView>();
                photonView.RPC("SetParent", RpcTarget.All, childid, parentid);
            }

            playernum++;
        }
    }
    
}
