using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteRaidmod : MonoBehaviour
{
    public Transform[] chairs = new Transform[3];
    public GameObject btnprefab;
    public Camera raidcam;
    public FriendPanel friendpanel;
    public Transform content;

    int party = 1;

    private void Start()
    {
        foreach(Transform chair in chairs)
        {
            GameObject InviteBtn = Instantiate(btnprefab, chair.position, Quaternion.identity, this.transform);
            InviteBtn.transform.position = raidcam.WorldToScreenPoint(chair.position + Vector3.up * 20);
            InviteBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                PanelManager.instance.PanelOpen("Invite");
                friendpanel.PartyFriend(content);
            });
        }
    }

}
