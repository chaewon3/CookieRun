using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRPC : MonoBehaviour
{
    [PunRPC]
    public void SetParent(int childid, int parentid)
    {
        GameObject child = PhotonView.Find(childid).gameObject;
        GameObject parent = PhotonView.Find(parentid).gameObject;
        child.transform.SetParent(parent.transform);
    }
}
