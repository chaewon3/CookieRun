using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeNamePanel : MonoBehaviour
{
    public TMP_InputField newNameInput;
    public TextMeshProUGUI errorMsg;
    public Button confirmBtn;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(ChangeButtnClick);
    }

    public void ChangeButtnClick()
    {
        int length = newNameInput.text.Length;

        if (length < 2 || length > 8)
        {
            errorMsg.text = "닉네임은 2~8글자로 설정해주세요.";
            newNameInput.text = string.Empty;
            return;
        }

        FirebaseManager.instance.UpdateUserName(newNameInput.text,
            () =>
            {
                this.gameObject.SetActive(false);
            },
            ()=>
            {
                errorMsg.text = "이미 존재하는 닉네임입니다.";
                newNameInput.text = string.Empty;
            });

    }

}
