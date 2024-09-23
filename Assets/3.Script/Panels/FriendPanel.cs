using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendPanel : MonoBehaviour
{
    FriendData Frienddata;
    List<UserData> friendData = new List<UserData>();
    List<UserData> RequestData = new List<UserData>();

    public GameObject friendList;
    public GameObject friendPrefab;
    public GameObject RequestList;
    public GameObject RequestPrefab;
    public GameObject partyPrefab;

    public TMP_InputField friendName;
    public Button submitBtn;

    private void Awake()
    {
        submitBtn.onClick.AddListener(FreindRequest);
    }

    private void OnEnable()
    {
        PanelRefresh();
        RequestList.transform.parent.parent.gameObject.SetActive(false);
    }

    void FreindRequest()
    {
        FirebaseManager.instance.FriendRequest(friendName.text);

        friendName.text = string.Empty;
    }

    public void PartyFriend(Transform content)
    {
        friendData.Clear();
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        FirebaseManager.instance.FriendListRefresh(() =>
        {
            Frienddata = FirebaseManager.instance.friendData;

            FirebaseManager.instance.GetFriend(Frienddata.friends, (data) => friendData.Add(data),
                () =>
                {
                    foreach (UserData data in friendData)
                    {
                        GameObject friend = Instantiate(partyPrefab, content);
                        friend.GetComponent<FriendInfo>().data = data;
                        friend.GetComponent<FriendInfo>().panel = this;
                    }
                });
        });


    }

    public void PanelRefresh()
    {
        friendData.Clear();
        RequestData.Clear();

        foreach (Transform child in friendList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RequestList.transform)
        {
            Destroy(child.gameObject);
        }
        FirebaseManager.instance.FriendListRefresh(()=>
        {
            Frienddata = FirebaseManager.instance.friendData;

            FirebaseManager.instance.GetFriend(Frienddata.friendRequest, (data) => RequestData.Add(data),
                () =>
                {
                    foreach (UserData data in RequestData)
                    {
                        if (data is null) continue;
                        GameObject friend = Instantiate(RequestPrefab, RequestList.transform);
                        friend.GetComponent<FriendInfo>().data = data;
                        friend.GetComponent<FriendInfo>().panel = this;
                    }
                });

            FirebaseManager.instance.GetFriend(Frienddata.friends, (data) => friendData.Add(data),
                () =>
                {
                    foreach (UserData data in friendData)
                    {
                        GameObject friend = Instantiate(friendPrefab, friendList.transform);
                        friend.GetComponent<FriendInfo>().data = data;
                        friend.GetComponent<FriendInfo>().panel = this;
                    }
                });

        });
    }

}
