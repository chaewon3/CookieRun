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

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance { get; private set; }

    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }
    public bool isInitialized { get; private set; } = false;

    public event Action onInit;

    public UserData userData;
    public DatabaseReference userRef;

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

            this.userData = userData;

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
            failurecallback?.Invoke("���������� �����ϴ�. ȸ�������� �������ּ���.");
        }
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


    public async void GetFriend(List<string> friends, Action<UserData> callback)
    {
        foreach (string friend in friends)
        {
            DatabaseReference Ref = DB.GetReference($"users/{friend}");
            DataSnapshot snapshot = await Ref.GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                UserData freindData = JsonConvert.DeserializeObject<UserData>(json);

                callback?.Invoke(freindData);
            }
            else // ģ�������� ������� ���? �ش� ģ�� �����
            {
                DatabaseReference userref = DB.GetReference($"users/{userData.userid}/friends");

                await userref.RemoveValueAsync();
            }
        }
    }


    public async void FriendRequest(string freindname)
    {
        DatabaseReference friendRef = DB.GetReference($"names/{freindname}");
        DataSnapshot friendsnapshot = await friendRef.GetValueAsync();
        string json = friendsnapshot.GetRawJsonValue();
        var jsonObj = JObject.Parse(json);
        string friendID = jsonObj.Properties().First().Value.ToString();

        print(friendID);
        DatabaseReference Ref = DB.GetReference($"users/{friendID}");
        DataSnapshot snapshot = await Ref.GetValueAsync();

        if (snapshot.Exists)
        {
            DatabaseReference requestRef = DB.GetReference($"users/{friendID}/friendRequest");
            var nameRef = requestRef.Child(userData.userid);
            await nameRef.SetValueAsync(userData.userid);
        }
        else 
        {
            //todo:�ش��Ѵ������� �����ϴ� �˸�����
            print("��������");
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
}
