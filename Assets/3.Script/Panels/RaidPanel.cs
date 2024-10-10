using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RaidPanel : MonoBehaviourPunCallbacks
{
    private const string chars = "ABCDEFGHIJKLMNOPQUSTUVWXYZ";
    string roomName;
    int playernum = 1;
    public AudioClip bgmclip;
    Dictionary<int, GameObject> playersCookie = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> playersnametag = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> ReadyToggle = new Dictionary<int, GameObject>();
    List<GameObject> InviteBtns = new List<GameObject>();
    List<GameObject> destroylist = new List<GameObject>();

    public Transform[] cookiePositions = new Transform[4];
    public GameObject playertagPrefab;
    public GameObject ReadyPrefab;

    [Header("UI Icon")]
    public Camera mainCam;
    public CinemachineVirtualCamera raidCam;
    public FriendPanel friendpanel;
    public GameObject btnprefab;
    public Transform content;
    public GameObject icons;
    public Button homeBtn;
    public Toggle ReadyBtn;
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI coinText;

    //todo : 이거도 플레이어의 대표쿠키 데이터가 여기로 들어오도록 SOX Data
    public CookieSO Data;

    [Header("Invite Panel")]
    public Button createParty;
    public Button LeaveBtn;
    public Button BackBtn;
    public Button CopyBtn;
    public TextMeshProUGUI RaidmodText;
    public TextMeshProUGUI PartyID;
    public GameObject PartyPanel;
    public GameObject PartyIdPanel;
    public GameObject[] PartyList = new GameObject[4];

    [Header("Party Attendance")]
    public GameObject AttendancePanel;
    public TMP_InputField PartyIDinput;
    public TextMeshProUGUI errorMsg;
    public Button AttendanceBtn;


    private void Awake()
    {
        createParty.onClick.AddListener(CreatePartyButtonClick);
        LeaveBtn.onClick.AddListener(LeaveBtnClick);
        AttendanceBtn.onClick.AddListener(() => AttendanceBtnClick(PartyIDinput.text));
        homeBtn.onClick.AddListener(()=> raidCam.Priority = 9);
        homeBtn.onClick.AddListener(LeaveBtnClick);
        BackBtn.onClick.AddListener(LeaveBtnClick);
        homeBtn.onClick.AddListener(() => StartCoroutine(leaveroom("Main")));
        BackBtn.onClick.AddListener(() => StartCoroutine(leaveroom("Play")));
        CopyBtn.onClick.AddListener(() => GUIUtility.systemCopyBuffer = roomName);
        ReadyBtn.onValueChanged.AddListener(ReadyBtnClick);
    }

    public IEnumerator Start()
    {
        yield return null;
        UserData data = FirebaseManager.instance.userData;
        CookieData dataCK = FirebaseManager.instance.cookieList.FindCookie(data.representCookie);

        GameObject cookie = Instantiate(Data.LobbyPrefab, cookiePositions[0]);
        cookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        var tag = Instantiate(playertagPrefab, icons.transform).GetComponent<PlayerTag>();
        tag.gameObject.transform.position = mainCam.WorldToScreenPoint(cookiePositions[0].position + Vector3.up * 30+Vector3.right*3);
        tag.name.text = FirebaseManager.instance.userData.username;
        tag.ATK.text = (dataCK.ATK + dataCK.DEF + dataCK.HP).ToString("N0");
        tag.localplayer = true; 
        GameObject Ready = Instantiate(ReadyPrefab, cookiePositions[0].position, Quaternion.identity, icons.transform);
        Ready.transform.position = mainCam.WorldToScreenPoint(cookiePositions[0].position + Vector3.up * 10);
        Ready.SetActive(false);

        PartyList[0].GetComponentInChildren<TextMeshProUGUI>().text = FirebaseManager.instance.userData.username;

        playersCookie.Add(0, cookie);
        playersnametag.Add(0, tag.gameObject);
        ReadyToggle.Add(0, Ready);

        for (int i = 1; i<4;i++)
        {
            GameObject InviteBtn = Instantiate(btnprefab, cookiePositions[i].position, Quaternion.identity, icons.transform);
            InviteBtn.transform.position = mainCam.WorldToScreenPoint(cookiePositions[i].position + Vector3.up * 20);
            InviteBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                icons.SetActive(false);
                PanelManager.instance.PanelOpen("Invite");
                friendpanel.PartyFriend(content);
                SoundManager.instance.BtnClick();
            });
            InviteBtns.Add(InviteBtn);
        }
    }

    private void OnEnable()
    {
        UserData data = FirebaseManager.instance.userData;
        heartText.text = $"{data.heart}/150";
        coinText.text = data.coin.ToString("N0");
        BGMManager.instance.BGMchange(bgmclip);
    }
    private void OnDisable()
    {
        BGMManager.instance.LobbyBGM();
    }

    public void CreatePartyButtonClick()
    {
        if (PhotonNetwork.InRoom) return;
        char[] stringChars = new char[6];
        System.Random random = new System.Random();

        for(int i = 0; i< 6;i++)
        {
            int index = random.Next(chars.Length);
            stringChars[i] = chars[index];
        }

        roomName = new string(stringChars);

        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 4 });
        base.OnEnable();
        PartyPanel.SetActive(false);
        PartyIdPanel.SetActive(true);
        LeaveBtn.gameObject.SetActive(true);
        RaidmodText.text = roomName;
        PartyID.text = roomName;
    }

    void LeaveBtnClick()
    {
        PartyPanel.SetActive(true);
        PartyIdPanel.SetActive(false);
        LeaveBtn.gameObject.SetActive(false);
        RaidmodText.text = "레이드 모드";
        playernum = 1;

        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }
    IEnumerator leaveroom(string panel)
    {
        while(PhotonNetwork.InRoom)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        PanelManager.instance.PanelChange(panel);
    }

    public void AttendanceBtnClick(string roomname)
    {
        PhotonNetwork.JoinRoom(roomname);
        base.OnEnable();
        roomName = roomname;
    }

    public override void OnJoinedRoom()
    {
        PartyPanel.SetActive(false);
        PartyIdPanel.SetActive(true);
        LeaveBtn.gameObject.SetActive(true);
        RaidmodText.text = roomName;
        PartyID.text = roomName;
        AttendancePanel.SetActive(false);
        print(PhotonNetwork.CurrentRoom.Name);
        Player localPlayer = PhotonNetwork.LocalPlayer;
        UserData data = FirebaseManager.instance.userData;
        Gamemanager.instance.playersData.Add(localPlayer.ActorNumber, data);
        Gamemanager.instance.playersCookie.Add(localPlayer.ActorNumber, FirebaseManager.instance.cookieList.FindCookie(data.representCookie));

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player == PhotonNetwork.LocalPlayer) continue;
            FirebaseManager.instance.GetPartyData(player.NickName, (data) =>
            {
                Gamemanager.instance.playersData.Add(player.ActorNumber, data);
                CookieData cookiedata;
                FirebaseManager.instance.GetCookieData(data.userid, data.representCookie, (cookiedata) =>
                {
                    Gamemanager.instance.playersCookie.Add(player.ActorNumber, cookiedata);
                    JoinPlayer(player);
                });
            });
        }
        // 방에 입장했을 때, 방장의 씬 로드 여부에 따라 함께 씬 로드
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnLeftRoom()
    {
        Gamemanager.instance.playersData.Clear();
        Gamemanager.instance.playersCookie.Clear();
        LeavePlayer();
        base.OnDisable();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print($"코드 {returnCode}, 오류 {message}");
        switch(returnCode)
        {
            case 32766: errorMsg.text = "파티가 가득 찼습니다."; break;
            default: errorMsg.text = "일치하는 파티가 없습니다."; break;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        FirebaseManager.instance.GetPartyData(newPlayer.NickName, (data) =>
        {
            Gamemanager.instance.playersData.Add(newPlayer.ActorNumber, data);
            CookieData cookiedata;
            FirebaseManager.instance.GetCookieData(data.userid, data.representCookie, (cookiedata) =>
            {
                Gamemanager.instance.playersCookie.Add(newPlayer.ActorNumber, cookiedata);
                JoinPlayer(newPlayer);
            });
        });
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Gamemanager.instance.playersData.Remove(otherPlayer.ActorNumber);
        LeavePlayer();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("GameLoad") )
        {
            bool GameLoad = (bool)changedProps["GameLoad"];
            if (CheckReady("GameLoad"))
                LoadingManager.instance.SceneChange();
            return;
        }

        if (changedProps.ContainsKey("Ready"))
        {
            print("호출은 됨");
            bool isReady = (bool)changedProps["Ready"];
            if (targetPlayer != PhotonNetwork.LocalPlayer)
                ReadyToggle[targetPlayer.ActorNumber].gameObject.SetActive(isReady);
            if (isReady)
            {
                if (CheckReady("Ready"))
                    GameStart();
            }
        }
       
    }
    void JoinPlayer(Player player)
    {// todo : 플레이어 정보에서 쿠키 가져와야함
        PartyList[playernum].SetActive(true);
        PartyList[playernum].GetComponentInChildren<TextMeshProUGUI>().text = player.NickName;

        GameObject PlayerCookie = Instantiate(Data.LobbyPrefab, cookiePositions[playernum]);
        PlayerCookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        var tag = Instantiate(playertagPrefab, icons.transform).GetComponent<PlayerTag>();
        tag.gameObject.transform.position = mainCam.WorldToScreenPoint(cookiePositions[playernum].position + Vector3.up * 30 + Vector3.right * 3);
        playersCookie.Add(player.ActorNumber, PlayerCookie);
        playersnametag.Add(player.ActorNumber, tag.gameObject);

        GameObject Ready = Instantiate(ReadyPrefab, cookiePositions[playernum].position, Quaternion.identity, icons.transform);
        Ready.transform.position = mainCam.WorldToScreenPoint(cookiePositions[playernum].position + Vector3.up * 10);
        if (player.CustomProperties.TryGetValue("Ready", out object ready) && ready is bool IsReady)
            Ready.SetActive(IsReady);
        else
            Ready.SetActive(false);
        ReadyToggle.Add(player.ActorNumber, Ready);

        destroylist.Add(PlayerCookie);
        destroylist.Add(tag.gameObject);
        destroylist.Add(Ready);

        tag.name.text = player.NickName;
        CookieData data = Gamemanager.instance.playersCookie[player.ActorNumber];
        tag.ATK.text = (data.ATK + data.DEF + data.HP).ToString("N0");

        GameObject invitebtn = InviteBtns[playernum-1];
        invitebtn.SetActive(false);
        playernum++;
    }

    void LeavePlayer()
    {
        playersCookie.Clear();
        playersnametag.Clear();
        ReadyToggle.Clear();
        for (int i = destroylist.Count-1;i>=0;i--)
        {
            Destroy(destroylist[i]);
        }

        for(int i = 1; i<4;i++)
        {
            PartyList[i].SetActive(false);
        }

        playernum =1;
        foreach(var btn in InviteBtns)
        {
            btn.SetActive(true);
        }
        gameRoomRefresh();
    }

    void gameRoomRefresh()
    {
        if (!PhotonNetwork.InRoom) return;
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player == PhotonNetwork.LocalPlayer) continue;
            JoinPlayer(player);
        }
    }

    void ReadyBtnClick(bool isOn)
    {
        ReadyToggle[0].SetActive(isOn);
        Player localplayer = PhotonNetwork.LocalPlayer;
        Hashtable customProps = localplayer.CustomProperties;
        customProps["Ready"] = isOn;
        localplayer.SetCustomProperties(customProps);
    }

    bool CheckReady(string props)
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers != playernum) return false;
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if(player.CustomProperties.TryGetValue(props, out object isReady))
            {
                if(isReady is bool ready && !ready)
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

    void GameStart()
    {
        BGMManager.instance.fadeout();
        PanelManager.instance.RaidLoading();
        //로딩화면띄우고 씬 이동
        LoadingManager.instance.currentScene = "RaidScene";
        LoadingManager.instance.PhotonLoadgame(()=>
        {
            Player localplayer = PhotonNetwork.LocalPlayer;
            Hashtable customProps = localplayer.CustomProperties;
            customProps["GameLoad"] = true;
            localplayer.SetCustomProperties(customProps);
        }); 
    }
}
