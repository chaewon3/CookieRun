using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SignupPanel : MonoBehaviour
{
    public GameObject loginPanel;

    public TMP_InputField emailInput;
    public TMP_InputField pwInput;

    public Button signupBtn;
    public TextMeshProUGUI errorMsg;

    private void Awake()
    {
        signupBtn.onClick.AddListener(SignupBynClcik);
    }

    private void Start()
    {
        errorMsg.text = "";
    }

    void SignupBynClcik()
    {
        signupBtn.interactable = false;
        FirebaseManager.instance.Signup(emailInput.text, pwInput.text, (user) =>
        {
            signupBtn.interactable = true;
            loginPanel.SetActive(true);
            this.gameObject.SetActive(false);
        },
        () =>
        {
            signupBtn.interactable = true;
            errorMsg.text = "회원가입에 실패하셨습니다.";
        });
    }
}
