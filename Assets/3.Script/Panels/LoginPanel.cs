using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class LoginPanel : MonoBehaviour
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
                if (LoadingManager.instance == null)
                    print("????");
                LoadingManager.instance.Scenechange();
            },
            (msg) =>
            {
                errorMsg.text = msg;
                signupBtn.interactable = true;
                loginBtn.interactable = true;
            });
    }
}
