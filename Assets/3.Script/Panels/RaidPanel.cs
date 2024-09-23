using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RaidPanel : MonoBehaviourPunCallbacks
{
    private const string chars = "ABCDEFGHIJKLMNOPQUSTUVWXYZ";
    string roomName;

    public Transform[] cookiePositions = new Transform[4];
    public GameObject platertagPrefab;

    [Header("UI Icon")]
    public Camera raidcam;
    public FriendPanel friendpanel;
    public GameObject btnprefab;
    public Transform content;
    public GameObject icons;

    //todo : 이거도 플레이어의 대표쿠키 데이터가 여기로 들어오도록 SOX Data
    public CookieSO Data;

    [Header("Invite Panel")]
    public Button createParty;
    public Button LeaveBtn;
    public TextMeshProUGUI RaidmodText;
    public TextMeshProUGUI PartyID;
    public GameObject PartyPanel;
    public GameObject PartyIdPanel;

    [Header("Party Attendance")]
    public GameObject AttendancePanel;
    public TMP_InputField PartyIDinput;
    public TextMeshProUGUI errorMsg;
    public Button AttendanceBtn;


    private void Awake()
    {
        createParty.onClick.AddListener(CreatePartyButtonClick);
        LeaveBtn.onClick.AddListener(LeaveBtnClick);
        AttendanceBtn.onClick.AddListener(AttendanceBtnClick);
    }

    private void Start()
    { //todo : 쿠키리스트부터 만들고 거기서 가져와야할듯?
        GameObject cookie = Instantiate(Data.ModelPrefab, cookiePositions[0]);
        cookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        GameObject tag = Instantiate(platertagPrefab, icons.transform);
        tag.transform.position = raidcam.WorldToScreenPoint(cookiePositions[0].position + Vector3.up * 30+Vector3.right*3); ;

        for (int i = 1; i<4;i++)
        {
            GameObject InviteBtn = Instantiate(btnprefab, cookiePositions[i].position, Quaternion.identity, icons.transform);
            InviteBtn.transform.position = raidcam.WorldToScreenPoint(cookiePositions[i].position + Vector3.up * 20);
            InviteBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                icons.SetActive(false);
                PanelManager.instance.PanelOpen("Invite");
                friendpanel.PartyFriend(content);
            });
        }
    }

    void CreatePartyButtonClick()
    {
        char[] stringChars = new char[6];
        System.Random random = new System.Random();

        for(int i = 0; i< 6;i++)
        {
            int index = random.Next(chars.Length);
            stringChars[i] = chars[index];
        }

        roomName = new string(stringChars);

        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 4 });
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
        PhotonNetwork.LeaveRoom();
    }

    void AttendanceBtnClick()
    {
        PhotonNetwork.JoinRoom(PartyIDinput.text);
        roomName = PartyIDinput.text;
    }

    public override void OnJoinedRoom()
    {
        PartyPanel.SetActive(false);
        PartyIdPanel.SetActive(true);
        LeaveBtn.gameObject.SetActive(true);
        RaidmodText.text = roomName;
        PartyID.text = roomName;
        AttendancePanel.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        switch(returnCode)
        {
            case 32766: errorMsg.text = "파티가 가득 찼습니다."; break;
            default: errorMsg.text = "일치하는 파티가 없습니다."; break;
        }
    }

}
