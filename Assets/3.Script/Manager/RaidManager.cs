using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RaidManager : MonoBehaviourPunCallbacks
{
    public static RaidManager instance { get; set; }

    public GameObject[] PlayerPositions;
    public GameObject[] BossPositions;
    public InputActionAsset playerMove;
    public CinemachineVirtualCamera camera;
    public GameObject warfPanel;
    public GameObject BossPosition;
    public Transform capturePLPosition;
    public GameObject UiPanel;
    public GameObject ResultPanel;
    public GameObject completePanel;
    public TextMeshProUGUI timeText;
    public AudioClip completeclip;
    public GameObject raqidStart;
    int playerEnter = 0;
    int loaclPLNum;
    bool leftgame;
    GameObject localPlayer;
    GameObject boss;

    public bool isPlay;
    public bool ClearGame { get; set; } = false;
    public float maxTime { get; set; } = 360;
    public float time;
    public int Damage { get; set; } = 0;
    public int DamageRank { get; set; }

    public Dictionary<int, UserData> players { get; private set; } = new Dictionary<int, UserData>();
    public Dictionary<int, CookieData> playersCookie { get; private set; } = new Dictionary<int, CookieData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        time = maxTime;
        players = Gamemanager.instance.playersData;
        playersCookie = Gamemanager.instance.playersCookie;
        if (PhotonNetwork.IsMasterClient)
        {
            boss = PhotonNetwork.Instantiate("Jungle_Gorilla", BossPosition.transform.position, BossPosition.transform.rotation);
            Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;
            customProperties.Remove("Ready"); // 프로퍼티 제거
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
    }

    private IEnumerator Start()
    {
        CreatePlayer();
        raqidStart.SetActive(true);
        yield return new WaitForSeconds(1.05f);
        raqidStart.SetActive(false);
        Gamemanager.instance.canMove = true;
    }

    private void Update()
    {
        if (isPlay)
            time -= Time.deltaTime;
        if (time <= 0)
            StartCoroutine(EndGame());
        timeText.text = $"{((int)time / 60).ToString("D2")}:{((int)time % 60).ToString("D2")}";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (leftgame) return;
        leftgame = true;
        Gamemanager.instance.errorMsg = "플레이어의 연결이 끊겨 게임이 종료되었습니다.";
        PhotonNetwork.LeaveRoom();
        LoadingManager.instance.currentScene = "LobbyScene";
        LoadingManager.instance.SceneLoad();
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("IsDie"))
        {
            if(CheckALLDie("IsDie")) // 모든 캐릭터가 죽었다면
            {
                StartCoroutine(EndGame());
            }
        }
        if(changedProps.ContainsKey("Damage"))
        {
            Ranking();
        }
    }

    bool CheckALLDie(string props)
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.TryGetValue(props, out object isReady))
            {
                if (isReady is bool ready && !ready)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    void Ranking()
    {
        int[] Pl = new int[4];
        int index = 0;
        int localPl = 0;
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.TryGetValue("Damage", out object damage))
            {
                Pl[index] = (int)damage;
                if (player == PhotonNetwork.LocalPlayer)
                    localPl = (int)damage;
            }
            else
                return;
            index++;
        }
        bool change = true;
        while(change)
        {
            change = false;
            for (int i = 0; i<3; i++)
            {
                if (Pl[i] < Pl[i + 1])
                {
                    int temp = Pl[i];
                    Pl[i] = Pl[i + 1];
                    Pl[i + 1] = temp;
                    change = true;
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (Pl[i] == localPl)
            {
                DamageRank = i + 1;
                return;
            }
        }
        DamageRank = 3;
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

                var capturepl = Instantiate(data.Data.LobbyPrefab, capturePLPosition);
            }

            playernum++;
        }
    }

    public void CreateGhost()
    {
        FindObjectOfType<Enemy_Boss_Gorilla>().Players.Remove(this.gameObject.transform);
        Player localplayer = PhotonNetwork.LocalPlayer;
        Hashtable customprops = localplayer.CustomProperties;
        customprops["IsDie"] = true;
        localplayer.SetCustomProperties(customprops);
        var ghostCookie = PhotonNetwork.Instantiate("GhostCookie", localPlayer.transform.position, localPlayer.transform.rotation);
        
        int parentid = localPlayer.GetComponent<PhotonView>().ViewID;
        int childid = ghostCookie.gameObject.GetComponent<PhotonView>().ViewID;
        PhotonView photonView = localPlayer.GetComponent<PhotonView>();
        photonView.RPC("SetParent", RpcTarget.All, childid, parentid);
        localPlayer.GetComponent<PlayerMove>().Cookie = ghostCookie.GetComponent<ICookie>();
    }

    IEnumerator CheckReady()
    {
        warfPanel.SetActive(true);
        yield return new WaitForSeconds(10);
        PlayerMove player = localPlayer.GetComponent<PlayerMove>();
        player.Warf(BossPositions[loaclPLNum].transform);
        yield return new WaitForSeconds(6);
        isPlay = true;
    }

    public IEnumerator EndGame()
    {        
        leftgame = true;
        isPlay = false;
        localPlayer.GetComponent<PlayerMove>().enabled = false;
        Gamemanager.instance.canMove = false;
        //각자의 누적댜미지 프로퍼티로 전달
        Player localplayer = PhotonNetwork.LocalPlayer;
        Hashtable customprops = localplayer.CustomProperties;
        customprops["Damage"] = Damage;
        localplayer.SetCustomProperties(customprops);
        BGMManager.instance.fadeout();
        yield return new WaitForSeconds(3f);

        UiPanel.SetActive(false);
        completePanel.SetActive(true);
        SoundManager.instance.clipPlay(completeclip);
        yield return new WaitForSeconds(1.8f);
        completePanel.SetActive(false);
        ResultPanel.SetActive(true);
    }
}
