using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceIcon : MonoBehaviour
{
    public CookieData data;
    public CookiePanel cookiepanel;
    public Image icon;
    public Toggle toggle;
    public Animator anim;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.group = GetComponentInParent<ToggleGroup>();
        toggle.onValueChanged.AddListener(setcookie);
    }
    private void Start()
    {
        icon.sprite = data.Data.faceIcon;
    }

    public void setcookie(bool ison)
    {
        if(ison)
        {
            cookiepanel.dataRefresh(data);
            anim.SetTrigger("Selected");
        }
        anim.SetTrigger("Normal");
    }
}

