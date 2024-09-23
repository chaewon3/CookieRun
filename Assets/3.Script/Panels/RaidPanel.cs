using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidPanel : MonoBehaviour
{
    public Transform[] cookiePositions = new Transform[4];
    public GameObject platertagPrefab;

    public Camera raidcam;

    public FriendPanel friendpanel;
    public GameObject btnprefab;
    public Transform content;
    public GameObject icons;

    //todo : 이거도 플레이어의 대표쿠키 데이터가 여기로 들어오도록 SOX Data
    public CookieSO Data;

    private void Start()
    { //todo : 쿠키리스트부터 만들고 거기서 가져와야할듯?
        GameObject cookie = Instantiate(Data.ModelPrefab, cookiePositions[0]);
        cookie.GetComponent<Animator>().runtimeAnimatorController = Data.ChairAnim;
        GameObject tag = Instantiate(platertagPrefab, icons.transform);
        tag.transform.position = raidcam.WorldToScreenPoint(cookiePositions[0].position + Vector3.up * 30+Vector3.right*3); ;

        for (int i = 1; i<4;i++)
        {
            GameObject InviteBtn = Instantiate(btnprefab, cookiePositions[i].position, Quaternion.identity, icons.transform);
            InviteBtn.transform.position = raidcam.WorldToScreenPoint(cookiePositions[i].position + Vector3.up * 20);
            InviteBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                icons.SetActive(false);
                PanelManager.instance.PanelOpen("Invite");
                friendpanel.PartyFriend(content);
            });
        }
    }
}
