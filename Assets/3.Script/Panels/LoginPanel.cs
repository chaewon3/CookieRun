using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoginPanel : MonoBehaviourPunCallbacks
{
    public TMP_InputField emailInput;
    public  TMP_InputField pwInput;

    public Button signupBtn;
    public Button loginBtn;

    public TextMeshProUGUI errorMsg;

    private void Awake()
    {
        loginBtn.onClick.AddListener(LoginButtonClick);
    }

    private void OnEnable()
    {
        errorMsg.text = "";
        emailInput.text = string.Empty;
        pwInput.text = string.Empty;
    }

    void LoginButtonClick()
    {
        signupBtn.interactable = false;
        loginBtn.interactable = false;
        FirebaseManager.instance.Login(emailInput.text, pwInput.text,
            () =>
            {
                PhotonNetwork.LocalPlayer.NickName = FirebaseManager.instance.userData.username;
                PhotonNetwork.ConnectUsingSettings();
                LoadingManager.instance.SceneChange();
            },
            (msg) =>
            {
                errorMsg.text = msg;
                signupBtn.interactable = true;
                loginBtn.interactable = true;
            });
    }

    public override void OnConnectedToMaster()
    {
        print("立加窃");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("肺厚!");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("立加 给窃");
    }
}
