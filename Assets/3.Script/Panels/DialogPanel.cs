using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogPanel : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public Button cancleBtn;
    public Button confirmBtn;
        
    public void onDialog(string dialog, Action comfirmOnclick)
    {
        confirmBtn.onClick.RemoveAllListeners();
        cancleBtn.onClick.RemoveAllListeners();

        cancleBtn.onClick.AddListener(() => this.gameObject.SetActive(false));
        confirmBtn.onClick.AddListener(() => this.gameObject.SetActive(false));

        dialogText.text = dialog;
        confirmBtn.onClick.AddListener(() => comfirmOnclick());
    }
}
