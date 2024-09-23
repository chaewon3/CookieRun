using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
            RefuseBtn.onClick.AddListener(RefuseBtnClick);
        }
        if(inviteBtn != null)
        {
            inviteBtn.onClick.AddListener(inviteBtnClick);
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

    void inviteBtnClick()
    {

    }

}