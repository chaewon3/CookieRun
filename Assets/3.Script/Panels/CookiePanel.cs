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
    public TextMeshProUGUI coin;
    public AudioClip levelup;

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

        Level.text = data.currentlevel.ToString();
        Name.text = data.Data.name;
        Power.text = (data.ATK + data.DEF + data.HP).ToString("N0");
        ATK.text = data.ATK.ToString("N0");
        DEF.text = data.DEF.ToString("N0");
        HP.text = data.HP.ToString("N0");
        Power2.text = Power.text;
        int requsetcoin = (data.currentlevel / 5 + 1) * 200;
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
        coin.text = FirebaseManager.instance.userData.coin.ToString("N0");
    }

    public void levelupBtnClick()
    {
        if(data.currentlevel == 60)
        {
            PanelManager.instance.notice("레벨이 최대치입니다.");
            return;
        }
        int requestCoin = (data.currentlevel / 5 + 1) * 200;
        if(FirebaseManager.instance.userData.coin < requestCoin)
        {
            print(FirebaseManager.instance.userData.coin);
            PanelManager.instance.notice("보유한 코인이 부족합니다.");
            return;
        }

        SoundManager.instance.clipPlay(levelup);
        cookiePrefab.GetComponent<Animator>().SetTrigger("LevelUp");
        data.levelUP();
        FirebaseManager.instance.userData.coin -= requestCoin;

        FirebaseManager.instance.UpdateUserData();
        FirebaseManager.instance.cookieLevelUp(data,() =>
        {
            FirebaseManager.instance.cookieList.UpdateCookie(data);
            dataRefresh(data);
        });
    }
}
