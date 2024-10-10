using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class RaidResultPanel : MonoBehaviour
{
    public GameObject CapturePL;
    public TextMeshProUGUI timeText;
    public Color color;
    public Image BG;
    public GameObject Fail;
    public GameObject Win;

    public TextMeshProUGUI cointext;
    public TextMeshProUGUI exptext;

    Animator cookieanim;

    public Sprite goldTrophy;
    public Sprite silverTrophy;
    public Sprite bronzeTrophy;

    public Sprite DamageIcon;
    public Sprite LiveIcon;

    public List<Trophy> trophies = new List<Trophy>();
    int index = 0;
    int bonous = 0;
    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        cookieanim = CapturePL.GetComponentInChildren<Animator>();

        if (RaidManager.instance.ClearGame)
        {
            animator.SetTrigger("Win");
            Win.SetActive(true);
        }
        else
        {
            animator.SetTrigger("Loose");
            BG.color = color;
            Fail.SetActive(true);
        }
    }
    private void Start()
    {
        if (RaidManager.instance.ClearGame)
            cookieanim.SetTrigger("Win");
        else
            cookieanim.SetTrigger("Loose");

        float time = RaidManager.instance.maxTime - RaidManager.instance.time;
        timeText.text = $"{(int)time / 60}:{(time % 60).ToString("F2")}";

        if (!RaidManager.instance.ClearGame) return;

        if (!Gamemanager.instance.IsDie)
        {
            trophies[index].GetTrophy(goldTrophy, LiveIcon, "∫“±º¿« ƒÌ≈∞");
            index++;
            bonous += 3;
        }
        switch(RaidManager.instance.DamageRank)
        {
            case 1: trophies[index].GetTrophy(goldTrophy, DamageIcon, "¿‘»˘ «««ÿ∑Æ"); bonous += 3; break;
            case 2: trophies[index].GetTrophy(silverTrophy, DamageIcon, "¿‘»˘ «««ÿ∑Æ"); bonous += 2; break;
            case 3: trophies[index].GetTrophy(bronzeTrophy, DamageIcon, "¿‘»˘ «««ÿ∑Æ"); bonous += 1; break;
        }
        index++;

        int coin = 5000 + 1000 * bonous;
        int exp = 300 + 50 * bonous;
        cointext.text = coin.ToString("N0");
        exptext.text = exp.ToString();


        FirebaseManager.instance.userData.coin += coin;
        FirebaseManager.instance.userData.GetExp(exp);
        FirebaseManager.instance.UpdateUserData();
    }

    public void ExitGame()
    {
        if(PhotonNetwork.InRoom)
           PhotonNetwork.LeaveRoom();
        LoadingManager.instance.currentScene = "LobbyScene";
        LoadingManager.instance.SceneLoad();
    }
}
