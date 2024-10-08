using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookiePanel : MonoBehaviour
{
    public CookieData data;
    public Button levelupBtn;
    public GameObject CookieList;
    public GameObject faceIconPrefab;

    public GameObject cookiePosition;
    public GameObject cookiePrefab;

    [Header("Info")]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI ATK;
    public TextMeshProUGUI DEF;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Power2;
    public TextMeshProUGUI requestCoin;

    [Header("Skills")]
    public TextMeshProUGUI ATKname;
    public TextMeshProUGUI ATKtext;

    public Image skillImg;
    public TextMeshProUGUI skillname;
    public TextMeshProUGUI skilltext;

    public TextMeshProUGUI dashname;
    public TextMeshProUGUI dashtext;

    public Image ultimateImg;
    public TextMeshProUGUI ultimatename;
    public TextMeshProUGUI ultimatetext;

    private void Awake()
    {
        levelupBtn.onClick.AddListener(levelupBtnClick);
    }

    private void OnEnable()
    {
        bool firstcookie = false;
        Toggle toggle = null;
        foreach(CookieData cookie in FirebaseManager.instance.cookieList.cookies)
        {
            var faceIcon = Instantiate(faceIconPrefab, CookieList.transform).GetComponent<FaceIcon>();
            faceIcon.data = cookie;
            faceIcon.cookiepanel = this;
            if(!firstcookie)
            {
                toggle = faceIcon.GetComponent<Toggle>();
                firstcookie = true;
            }
        }
        toggle.isOn = true;
    }

    private void OnDisable()
    {
        for (int i = CookieList.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(CookieList.transform.GetChild(i).gameObject);
        }
    }
    public void dataRefresh(CookieData data)
    {
        if (cookiePrefab != null) Destroy(cookiePrefab);
        this.data = data;

        cookiePrefab = Instantiate(data.Data.LobbyPrefab, cookiePosition.transform);

        Level.text = data.level.ToString();
        Name.text = data.Data.name;
        Power.text = (data.ATK + data.DEF + data.HP).ToString();
        ATK.text = data.ATK.ToString();
        DEF.text = data.DEF.ToString();
        HP.text = data.HP.ToString();
        Power2.text = Power.text;
        int requsetcoin = (data.level / 5 + 1) * 200;
        requestCoin.text = requsetcoin.ToString();

        ATKname.text = data.Data.ATKName;
        ATKtext.text = data.Data.ATKDes;

        skillImg.sprite = data.Data.skillImg;
        skillname.text = data.Data.skillName;
        skilltext.text = data.Data.skillDes;

        dashname.text = data.Data.dashName;
        dashtext.text = data.Data.dashDes;

        ultimateImg.sprite = data.Data.ultimateImg;
        ultimatename.text = data.Data.ultimateName;
        ultimatetext.text = data.Data.ultimateDes;
    }

    public void levelupBtnClick()
    {
        if(data.level == 60)
        {
            PanelManager.instance.notice("레벨이 최대치입니다.");
            return;
        }
        int requestCoin = (data.level / 5 + 1) * 200;
        if(FirebaseManager.instance.userData.coin < requestCoin)
        {
            PanelManager.instance.notice("보유한 코인이 부족합니다.");
            return;
        }

        // 유저정보 리셋
        data.levelUP();
        // 쿠키리스트 리셋
        dataRefresh(data);
    }
}
