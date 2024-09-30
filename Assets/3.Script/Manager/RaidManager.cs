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
    public GameObject localPlayer;
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
                var playerInput = PlayerPositions[playernum].GetComponent<PlayerInput>();
                CookieBase localCK = Instantiate(data.Data.ModelPrefab, PlayerPositions[playernum].transform).GetComponent<CookieBase>();
                //PhotonNetwork.Instantiate
                localCK.Cookie = data;
                camera.Follow = PlayerPositions[playernum].transform;

                playerInput.enabled = true;
                PlayerPositions[playernum].GetComponent<PlayerMove>().enabled = true;
            }
            else
            {
                GameObject CK = Instantiate(data.Data.LobbyPrefab, PlayerPositions[playernum].transform);
            }
            playernum++;
        }
    }
}
