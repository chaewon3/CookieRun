using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRPC : MonoBehaviour
{
    private void Awake()
    {
        int playernum = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (player == photonView.Owner)
            {
                HPBarPanel.instance.SetRPCHP(this.transform, player);
                FindObjectOfType<Enemy_Boss_Gorilla>().Players[playernum] = this.gameObject.transform;
            }
            playernum++;
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

    public void PlayerMove(Vector3 direction)
    {
        StartCoroutine(Move(direction));
    }

    public IEnumerator Move(Vector3 direction)
    {
        CharacterController cc = GetComponent<CharacterController>();
        float time = 0.5f;
        while(time >= 0)
        {
            cc.Move(direction * Time.deltaTime);
            yield return null;
            time -= Time.deltaTime;
        }
    }
}
