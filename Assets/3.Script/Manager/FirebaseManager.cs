using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance { get; private set; }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }
    public bool isInitialized { get; private set; } = false;

    public event Action onInit;

    public UserData userData;
    public CookieList cookieList;
    public DatabaseReference userRef;
    public DatabaseReference inviteRef;

    public FriendData friendData = new FriendData();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
        
        if(status == DependencyStatus.Available)
        {
            App = FirebaseApp.DefaultInstance;
            Auth = FirebaseAuth.DefaultInstance;
            DB = FirebaseDatabase.DefaultInstance;
            isInitialized = true;
        }
        else
        {
            Debug.Log("����");
        }
    }

    public async void Signup(string email, string pw, Action<FirebaseUser> callback = null, Action failurecallback = null)
    {
        try
        {
            var result = await Auth.CreateUserWithEmailAndPasswordAsync(email, pw);

            userRef = DB.GetReference($"users/{result.User.UserId}");
            UserData userData = new UserData(result.User.UserId);
            string userDataJson = JsonConvert.SerializeObject(userData);
            await userRef.SetRawJsonValueAsync(userDataJson);

            CookieList cookieList = new CookieList(Cookies.BraveCookie);
            foreach(CookieData cookie in cookieList.cookies)
            {
                DatabaseReference cookieRef = DB.GetReference($"cookieList/{result.User.UserId}/{cookie.cookie.ToString()}");
                string cookielistJson = JsonConvert.SerializeObject(cookie);
                await cookieRef.SetRawJsonValueAsync(cookielistJson);
            }

            callback?.Invoke(result.User);

        }
        catch(FirebaseException fe)
        {
            failurecallback?.Invoke();
        }
    }

    public async void Login(string email, string pw, Action callback = null, Action<string> failurecallback = null)
    {
        try
        {
            var result = await Auth.SignInWithEmailAndPasswordAsync(email, pw);

            userRef = DB.GetReference($"users/{result.User.UserId}");
            DataSnapshot userDataValues = await userRef.GetValueAsync();
            if (userDataValues.Exists)
            {
                string json = userDataValues.GetRawJsonValue();
                userData = JsonConvert.DeserializeObject<UserData>(json);

                inviteRef = DB.GetReference($"invite/{result.User.UserId}");
                await inviteRef.SetValueAsync(true);
                inviteRef.ChildAdded += ReceiveInvitation;

                DatabaseReference cookieRef = DB.GetReference($"cookieList/{result.User.UserId}");
                DataSnapshot cookiesnap = await cookieRef.GetValueAsync();
                foreach(var childsnap in cookiesnap.Children)
                {
                    string cookiejson = childsnap.GetRawJsonValue();
                    CookieData cookiedata = JsonConvert.DeserializeObject<CookieData>(cookiejson);
                    cookieList.AddCookie(cookiedata);
                }
            }
            else
            {
                throw new Exception();
            }

            callback?.Invoke();
        }
        catch (FirebaseException fe)
        {
            failurecallback?.Invoke("�α��ο� �����߽��ϴ�.");
        }
        catch (Exception e)
        {
            print(e.Message);
            failurecallback?.Invoke("���������� �����ϴ�. ȸ�������� �������ּ���.");
        }
    }

    private async void OnApplicationQuit()
    {
        await inviteRef.RemoveValueAsync();
    }

    public async void FriendListRefresh(Action callback)
    {
        friendData.friends.Clear();
        friendData.friendRequest.Clear();

        DatabaseReference friendListRef = DB.GetReference($"friendList/{userData.userid}");
        DataSnapshot friendDataValues = await friendListRef.GetValueAsync();
        if (friendDataValues.Exists)
        {
            foreach (var child in friendDataValues.Children)
            {
                friendData.friends.Add(child.Key);
            }
        }
        DatabaseReference requestListRef = DB.GetReference($"requestList/{userData.userid}");
        DataSnapshot requestDataValues = await requestListRef.GetValueAsync();
        if (requestDataValues.Exists)
        {
            foreach (var child in requestDataValues.Children)
            {
                friendData.friendRequest.Add(child.Key);
            }
        }
        callback?.Invoke();
    }

    public async void UpdateUserName(string name, Action callback = null, Action failureback = null)
    {
        DatabaseReference Ref = DB.GetReference($"names/{name}");
        DataSnapshot snapshot = await Ref.GetValueAsync();
        if(!snapshot.Exists)
        {
            string refKey = nameof(userData.username);

            var nameRef = Ref.Child(userData.userid);
            await nameRef.SetValueAsync(userData.userid);

            var userref = userRef.Child(refKey);
            await userref.SetValueAsync(name);

            callback?.Invoke();
        }
        else
        {
            failureback?.Invoke();
        }        
    }

    public async void GetFriend(List<string> friends, Action<UserData> addList, Action callback)
    {
        foreach (string friend in friends)
        {
            DatabaseReference Ref = DB.GetReference($"users/{friend}");
            DataSnapshot snapshot = await Ref.GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                UserData freindData = JsonConvert.DeserializeObject<UserData>(json);
                if(freindData != null)
                    addList?.Invoke(freindData);
            }
            else // todo : ���߿� ���� ģ�������� ������� ���? �ش� ģ�� �����
            {
                //DatabaseReference userref = DB.GetReference($"friend/{userData.userid}/friends");
                //await userref.RemoveValueAsync();
            }
        }
        callback?.Invoke();
    }

    public async void FriendRequest(string freindname)
    {
        //�̸����ε� ���̺��� �ش��ϴ� ����id ������
        DatabaseReference friendRef = DB.GetReference($"names/{freindname}");
        DataSnapshot friendsnapshot = await friendRef.GetValueAsync();

        string json = friendsnapshot.GetRawJsonValue();
        var jsonObj = JObject.Parse(json);
        string friendID = jsonObj.Properties().First().Value.ToString();

        //������ id�� �� ������ ģ����û��Ͽ� ����
        DatabaseReference Ref = DB.GetReference($"requestList/{friendID}");
        DataSnapshot snapshot = await Ref.GetValueAsync();

        if (friendsnapshot.Exists)
        {
            DatabaseReference requestRef = DB.GetReference($"requestList/{friendID}");
            var nameRef = requestRef.Child(userData.userid);
            await nameRef.SetValueAsync(userData.userid);
        }
        else 
        {
            //todo:�ش��Ѵ������� �����ϴ� �˸�����
            print("��������");
        }
    }

    public async void FriendRequestAccept(string friendid, Action callback)
    {
        DatabaseReference requestListRef = DB.GetReference($"requestList/{userData.userid}/{friendid}");
        await requestListRef.RemoveValueAsync();


        DatabaseReference friendListRef = DB.GetReference($"friendList/{userData.userid}");
        var friendRef = friendListRef.Child(friendid);
        await friendRef.SetValueAsync(friendid);

        DatabaseReference friendsListRef = DB.GetReference($"friendList/{friendid}");
        var friendsRef = friendsListRef.Child(userData.userid);
        await friendsRef.SetValueAsync(userData.userid);

        callback?.Invoke();
    }
    public async void FriendRequestRefuse(string friendid, Action callback)
    {
        DatabaseReference requestListRef = DB.GetReference($"requestList/{userData.userid}/{friendid}");

        await requestListRef.RemoveValueAsync();
        callback?.Invoke();
    }

    public async void InviteFriend(string friendid)
    {
        DatabaseReference Ref = DB.GetReference($"invite/{friendid}");
        DataSnapshot snapshot = await Ref.GetValueAsync();
        if(snapshot.Exists)
        {
            var nameRef = Ref.Child(userData.username);
            await nameRef.SetValueAsync(PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            // ������ ���������� �ʽ��ϴ�.
        }
    }

    public async void ReceiveInvitation(object sender, ChildChangedEventArgs args)
    {
        string dialog = $"<color=#DB552F>{args.Snapshot.Key}</color>���� <color=#1C68BF>���̵� ���</color> ��Ƽ�� �ʴ��߾��.";
        PanelManager.instance.Invitation(dialog, args.Snapshot.Value.ToString());
        await inviteRef.Child(args.Snapshot.Key).RemoveValueAsync();
    }

    public async void GetPartyData(string username, Action<UserData> callback)
    {
        DatabaseReference friendRef = DB.GetReference($"names/{username}");
        DataSnapshot friendsnapshot = await friendRef.GetValueAsync();
        string json = friendsnapshot.GetRawJsonValue();
        var jsonObj = JObject.Parse(json);
        string friendID = jsonObj.Properties().First().Value.ToString();
        DatabaseReference Ref = DB.GetReference($"users/{friendID}");
        DataSnapshot snapshot = await Ref.GetValueAsync();

        if(snapshot.Exists)
        {
            string userjson = snapshot.GetRawJsonValue();
            UserData data = JsonConvert.DeserializeObject<UserData>(userjson);
            print(data.username) ;
            callback?.Invoke(data);
        }
    }
    public async void GetStage(StageSO data, Action<StageData> callback)
    {
        DatabaseReference Ref = DB.GetReference($"stages/{userData.userid}/{data.Stagename}");
        DataSnapshot snapshot = await Ref.GetValueAsync();
        if(snapshot.Exists)
        { // �����Ͱ� ������ �����ͼ� ���
            string json = snapshot.GetRawJsonValue();
            StageData stage = JsonConvert.DeserializeObject<StageData>(json);

            callback?.Invoke(stage);
        }
        else
        { // �����Ͱ� ������ ���ο� ������ ���̺� ����
            StageData stagedata = new StageData(data);

            string stageDataJson = JsonConvert.SerializeObject(stagedata);
            await Ref.SetRawJsonValueAsync(stageDataJson);

            callback?.Invoke(stagedata);
        }
    }

    public async void GetCookieData(string userid, Cookies cookie, Action<CookieData> callback)
    {
        DatabaseReference Ref = DB.GetReference($"cookieList/{userData.userid}/{cookie.ToString()}");
        DataSnapshot snapshot = await Ref.GetValueAsync();
        if (snapshot.Exists)
        { // �����Ͱ� ������ �����ͼ� ���
            string json = snapshot.GetRawJsonValue();
            CookieData cookiedata = JsonConvert.DeserializeObject<CookieData>(json);
            cookiedata = new CookieData (DataManager.instance.GetCookieSO(cookiedata.cookie), cookiedata);
            print($"��Ű{cookiedata.cookie.ToString()}");
            callback?.Invoke(cookiedata);
        }
        else
        {
            print("����");
        }
    }
}
