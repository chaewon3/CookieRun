using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendPanel : MonoBehaviour
{
    UserData playerdata;
    List<UserData> friendData = new List<UserData>();
    List<UserData> RequestData = new List<UserData>();

    public GameObject friendList;
    public GameObject friendPrefab;
    public GameObject RequestList;
    public GameObject RequestPrefab;

    public TMP_InputField friendName;
    public Button submitBtn;

    private void Awake()
    {
        submitBtn.onClick.AddListener(FreindRequest);
    }

    private void OnEnable()
    {
        playerdata = FirebaseManager.instance.userData;

        FirebaseManager.instance.GetFriend(playerdata.friends, (data) => friendData.Add(data));

        foreach(UserData data in friendData)
        {
            GameObject friend = Instantiate(friendPrefab, friendList.transform);
            friend.GetComponent<FriendInfo>().data = data;
        }

        FirebaseManager.instance.GetFriend(playerdata.friendRequest, (data) => RequestData.Add(data));

        foreach (UserData data in RequestData)
        {
            GameObject friend = Instantiate(RequestPrefab, RequestList.transform);
            friend.GetComponent<FriendInfo>().data = data;
        }

    }

    private void OnDisable()
    {
        friendData.Clear();
    }

    void FreindRequest()
    {
        FirebaseManager.instance.FriendRequest(friendName.text);

        friendName.text = string.Empty;
    }

    
}
