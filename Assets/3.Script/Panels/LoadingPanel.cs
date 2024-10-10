using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    public  List<Sprite> sprList;
    public List<string> TipList;
    public List<Sprite> ElementFragiantList;
    public Image LoadImg;
    public TextMeshProUGUI Tips;
    public GameObject loading;
    public Slider loadingbar;
    public GameObject stageloading;

    [Header("RaidLoad")]
    public GameObject raidLoading;
    public GameObject portraitPrefab;
    public GameObject portraitContent;

    private void Update()
    {
        loading.transform.Rotate(-Vector3.forward, 90 * Time.deltaTime);
        loadingbar.value = LoadingManager.instance.progress;
    }

    private void OnEnable()
    {
        LoadImg.gameObject.SetActive(true);
        RandomImage();
    }


    public void RandomImage()
    {
        System.Random random = new System.Random();
        int index = random.Next(sprList.Count);
        LoadImg.sprite = sprList[index];

        index = random.Next(TipList.Count);
        Tips.text = TipList[index];

    }

    public void RaidLoading()
    {
        raidLoading.SetActive(true);
        stageloading.gameObject.SetActive(false);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            UserData data = Gamemanager.instance.playersData[player.ActorNumber];
            CookieData cookie = Gamemanager.instance.playersCookie[player.ActorNumber];
            var portrait = Instantiate(portraitPrefab, portraitContent.transform).GetComponent<PlayerTag>();
            portrait.name.text = data.username;
            int power = cookie.ATK + cookie.DEF + cookie.HP;
            portrait.ATK.text = power.ToString("N0");
            if (player == PhotonNetwork.LocalPlayer)
                portrait.localplayer = true;

        }
    }
}
