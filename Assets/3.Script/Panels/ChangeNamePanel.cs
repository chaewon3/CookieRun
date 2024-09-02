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
            errorMsg.text = "�г����� 2~8���ڷ� �������ּ���.";
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
                errorMsg.text = "�̹� �����ϴ� �г����Դϴ�.";
                newNameInput.text = string.Empty;
            });

    }

}
