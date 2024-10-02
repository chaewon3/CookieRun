using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRPC : MonoBehaviour
{
    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (player == photonView.Owner)
            {
                HPBarPanel.instance.SetRPCHP(this.transform, player);
            }
        }

    }

    [PunRPC]
    public void SetParent(int childid, int parentid)
    {
        GameObject child = PhotonView.Find(childid).gameObject;
        GameObject parent = PhotonView.Find(parentid).gameObject;
        child.transform.SetParent(parent.transform);
    }

    [PunRPC]
    public void WarfBoss(Vector3 position)
    {
        this.transform.position = position;
    }
}
