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
    public GameObject[] BossPositions;
    public InputActionAsset playerMove;
    public CinemachineVirtualCamera camera;
    public GameObject warfPanel;
    public GameObject BossPosition;

    int playerEnter = 0;
    int loaclPLNum;
    GameObject localPlayer;

    public Dictionary<int, UserData> players { get; private set; } = new Dictionary<int, UserData>();
    public Dictionary<int, CookieData> playersCookie { get; private set; } = new Dictionary<int, CookieData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        players = Gamemanager.instance.playersData;
        playersCookie = Gamemanager.instance.playersCookie;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Jungle_Gorilla", BossPosition.transform.position, BossPosition.transform.rotation);
        }
    }

    private void Start()
    {
        CreatePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask targetlayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == targetlayer)
        {
            playerEnter++;

            if (playerEnter == 4)
                StartCoroutine(CheckReady());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!Gamemanager.instance.OnGame) return;
        LayerMask targetlayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == targetlayer)
        {
            playerEnter--;
            StopAllCoroutines();
            warfPanel.SetActive(false);
        }
    }

    void CreatePlayer()
    {
        int playernum = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            CookieData data = playersCookie[player.ActorNumber];
            if (player == PhotonNetwork.LocalPlayer)
            {
                localPlayer = PhotonNetwork.Instantiate("LocalPlayer", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation);
                var playerInput = localPlayer.AddComponent<PlayerInput>();
                playerInput.actions = playerMove;
                playerInput.defaultControlScheme = "PC";
                playerInput.defaultActionMap = "PlayerAction";
                playerInput.ActivateInput();
                localPlayer.AddComponent<PlayerMove>();

                CookieBase localCK = PhotonNetwork.Instantiate($"{data.cookie.ToString()}", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation).GetComponent<CookieBase>();
                localCK.Cookie = data;
                camera.Follow = localPlayer.transform;

                int parentid = localPlayer.GetComponent<PhotonView>().ViewID;
                int childid = localCK.gameObject.GetComponent<PhotonView>().ViewID;
                PhotonView photonView = localPlayer.GetComponent<PhotonView>();
                photonView.RPC("SetParent", RpcTarget.All, childid, parentid);
                loaclPLNum = playernum;
            }

            playernum++;
        }
    }
    
    IEnumerator CheckReady()
    {
        warfPanel.SetActive(true);
        yield return new WaitForSeconds(10);
        PlayerMove player = localPlayer.GetComponent<PlayerMove>();
        player.Warf(BossPositions[loaclPLNum].transform);
    }
}
