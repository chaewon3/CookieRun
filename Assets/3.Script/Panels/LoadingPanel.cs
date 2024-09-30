using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LoadingPanel : MonoBehaviour
{
    public  List<Sprite> sprList;
    public List<Sprite> ElementFragiantList;
    public Image LoadImg;

    [Header("RaidLoad")]
    public GameObject raidLoading;
    public GameObject portraitPrefab;
    public GameObject portraitContent;

    private void OnEnable()
    {
        LoadImg.gameObject.SetActive(true);
        raidLoading.SetActive(false);
        RandomImage();
    }


    public void RandomImage()
    {
        System.Random random = new System.Random();
        int index = random.Next(sprList.Count);
        LoadImg.sprite = sprList[index];
    }

    public void RaidLoading()
    {
        LoadImg.gameObject.SetActive(false);
        raidLoading.SetActive(true);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            UserData data = Gamemanager.instance.playersData[player.ActorNumber];
            var portrait = Instantiate(portraitPrefab, portraitContent.transform).GetComponent<PlayerTag>();
            portrait.name.text = data.username;
            if (player == PhotonNetwork.LocalPlayer)
                portrait.localplayer = true;

        }
    }
}
