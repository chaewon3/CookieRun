using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRPC : MonoBehaviour
{

    PhotonView photonview;
    int playernum = 0;

    private void Awake()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (player == photonView.Owner)
            {
                HPBarPanel.instance.SetRPCHP(this.transform, player);
                return;
            }
            playernum++;
        }
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FindObjectOfType<Enemy_Boss_Gorilla>() != null);
        FindObjectOfType<Enemy_Boss_Gorilla>().Players.Add(this.gameObject.transform);
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
