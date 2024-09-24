using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System;

public class RaidPanel : MonoBehaviourPunCallbacks
{
    private const string chars = "ABCDEFGHIJKLMNOPQUSTUVWXYZ";
    string roomName;
    int playernum = 1;
    Dictionary<int, GameObject> playersCookie = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> playersnametag = new Dictionary<int, GameObject>();
    List<GameObject> InviteBtns = new List<GameObject>();
    List<GameObject> destroylist = new List<GameObject>();

    public Transform[] cookiePositions = new Transform[4];
    public GameObject platertagPrefab;

    [Header("UI Icon")]
    public Camera mainCam;
    public CinemachineVirtualCamera raidCam;
    public FriendPanel friendpanel;
    public GameObject btnprefab;
    public Transform content;
    public GameObject icons;
    public Button homeBtn;

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
    }

    public void Start()
    { //todo : 쿠키리스트부터 만들고 거기서 가져와야할듯?
        GameObject cookie = Instantiate(Data.ModelPrefab, cookiePositions[0]);
        cookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        var tag = Instantiate(platertagPrefab, icons.transform).GetComponent<PlayerTag>();
        tag.gameObject.transform.position = mainCam.WorldToScreenPoint(cookiePositions[0].position + Vector3.up * 30+Vector3.right*3);
        tag.name.text = FirebaseManager.instance.userData.username;
        PartyList[0].GetComponentInChildren<TextMeshProUGUI>().text = FirebaseManager.instance.userData.username;

        playersCookie.Add(0, cookie);
        playersnametag.Add(0, tag.gameObject);
                
        for (int i = 1; i<4;i++)
        {
            GameObject InviteBtn = Instantiate(btnprefab, cookiePositions[i].position, Quaternion.identity, icons.transform);
            InviteBtn.transform.position = mainCam.WorldToScreenPoint(cookiePositions[i].position + Vector3.up * 20);
            InviteBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                icons.SetActive(false);
                PanelManager.instance.PanelOpen("Invite");
                friendpanel.PartyFriend(content);
            });
            InviteBtns.Add(InviteBtn);
        }
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

        gameRoomRefresh();
        // 방에 입장했을 때, 방장의 씬 로드 여부에 따라 함께 씬 로드
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnLeftRoom()
    {
        LeavePlayer();
        base.OnDisable();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        switch(returnCode)
        {
            case 32766: errorMsg.text = "파티가 가득 찼습니다."; break;
            default: errorMsg.text = "일치하는 파티가 없습니다."; break;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UserData partydata = new UserData();
        FirebaseManager.instance.GetPartyData(newPlayer.NickName, (data) => partydata = data);
        Gamemanager.instance.players.Add(newPlayer.ActorNumber, partydata);
        JoinPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Gamemanager.instance.players.Remove(otherPlayer.ActorNumber);
        LeavePlayer();
    }

    void JoinPlayer(Player player)
    {// todo : 플레이어 정보에서 쿠키 가져와야함
        PartyList[playernum].SetActive(true);
        PartyList[playernum].GetComponentInChildren<TextMeshProUGUI>().text = player.NickName;

        GameObject PlayerCookie = Instantiate(Data.ModelPrefab, cookiePositions[playernum]);
        PlayerCookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        var tag = Instantiate(platertagPrefab, icons.transform).GetComponent<PlayerTag>();
        tag.gameObject.transform.position = mainCam.WorldToScreenPoint(cookiePositions[playernum].position + Vector3.up * 30 + Vector3.right * 3);
        playersCookie.Add(player.ActorNumber, PlayerCookie);
        playersnametag.Add(player.ActorNumber, tag.gameObject);

        destroylist.Add(PlayerCookie);
        destroylist.Add(tag.gameObject);

        tag.name.text = player.NickName;

        GameObject invitebtn = InviteBtns[playernum-1];
        invitebtn.SetActive(false);
        playernum++;
    }

    void LeavePlayer()
    {
        playersCookie.Clear();
        playersnametag.Clear();
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
}
