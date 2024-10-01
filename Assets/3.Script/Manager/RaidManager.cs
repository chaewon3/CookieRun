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

    int playerEnter = 0;
    int loaclPLNum;
    PhotonView photonView;

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
        CreatePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        LayerMask targetlayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == targetlayer)
        {
            playerEnter++;
            StartCoroutine(CheckReady());
        }

        print($"{playerEnter}현재 사람수");
    }

    private void OnTriggerExit(Collider other)
    {
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
                GameObject localPL = PhotonNetwork.Instantiate("LocalPlayer", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation);
                var playerInput = localPL.AddComponent<PlayerInput>();
                playerInput.actions = playerMove;
                playerInput.defaultControlScheme = "PC";
                playerInput.defaultActionMap = "PlayerAction";
                playerInput.ActivateInput();
                localPL.AddComponent<PlayerMove>();

                CookieBase localCK = PhotonNetwork.Instantiate($"{data.cookie.ToString()}", PlayerPositions[playernum].transform.position, PlayerPositions[playernum].transform.rotation).GetComponent<CookieBase>();
                localCK.Cookie = data;
                camera.Follow = localPL.transform;

                int parentid = localPL.GetComponent<PhotonView>().ViewID;
                int childid = localCK.gameObject.GetComponent<PhotonView>().ViewID;
                photonView = localPL.GetComponent<PhotonView>();
                photonView.RPC("SetParent", RpcTarget.All, childid, parentid);
                loaclPLNum = playernum;
            }

            playernum++;
        }
    }
    
    IEnumerator CheckReady()
    {
        if(playerEnter == 4)
        {
            warfPanel.SetActive(true);
            yield return new WaitForSeconds(7);
            photonView.RPC("WarfBoss", RpcTarget.All, BossPositions[loaclPLNum].transform.position);
        }
    }
}
