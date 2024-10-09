using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class FriendInfo : MonoBehaviour
{
    [HideInInspector]
    public UserData data;
    [HideInInspector]
    public FriendPanel panel;

    public TextMeshProUGUI Level;
    public TextMeshProUGUI name;

    public Button AcceptBtn;
    public Button RefuseBtn;
    public Button inviteBtn;

    private void Start()
    {
        Level.text = data.level.ToString();
        name.text = data.username;

        if(AcceptBtn != null && RefuseBtn != null)
        {
            AcceptBtn.onClick.AddListener(AcceptBtnClick);
            AcceptBtn.onClick.AddListener(() => SoundManager.instance.BtnClick()); 
            RefuseBtn.onClick.AddListener(RefuseBtnClick);
            RefuseBtn.onClick.AddListener(() => SoundManager.instance.BtnClick());
        }
        if (inviteBtn != null)
        {
            inviteBtn.onClick.AddListener(() => StartCoroutine(inviteBtnClick()));
            inviteBtn.onClick.AddListener(() => SoundManager.instance.BtnClick());
        }
    }

    void AcceptBtnClick()
    {
        FirebaseManager.instance.FriendRequestAccept(data.userid, () => panel.PanelRefresh());
    }

    void RefuseBtnClick()
    {
        FirebaseManager.instance.FriendRequestRefuse(data.userid, () => panel.PanelRefresh());
    }

    IEnumerator inviteBtnClick()
    {
        PanelManager.instance.CreateParty();
        while(!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        FirebaseManager.instance.InviteFriend(data.userid);
        inviteBtn.interactable = false;
    }

}