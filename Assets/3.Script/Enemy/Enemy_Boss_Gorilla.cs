using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Cinemachine;

using RandomNum = UnityEngine.Random;
public class Enemy_Boss_Gorilla : MonoBehaviourPunCallbacks, IEnemy
{
    public EnemySO data;
    public int ATK { get; set; }
    int HP;
    int DEF;
    public float HPPer => (float)HP / data.HP;

    Animator anim;
    public bool canMove = false;
    float atkCT;
    int statelength;
    public Transform transform;
    public Transform[] Players = new Transform[4];
    public Transform TargetPlayer;
    public CinemachineVirtualCamera cutscene;
    public BoxCollider boxCollider;
    public SphereCollider sphereCollider;
    public SphereCollider dropCollider;
    State currentState;

    float distance;
    Rigidbody rig;
    PhotonView photonview;

    enum State
    {
        SkillRush,
        //SkillThreat,
        //SkillEat,
        SkillSmash,
        SkillDrop
    }

    private void Start()
    {
        statelength = Enum.GetValues(typeof(State)).Length;
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        photonview = GetComponent<PhotonView>();
        StartCoroutine(Targeting());
        boxCollider.GetComponent<AttackCollider>().ATK = data.ATK;
        sphereCollider.GetComponent<AttackCollider>().ATK = (int)(data.ATK*1.5f);
    }

    private void Update()
    {
        // ������ Ŭ���̾�Ʈ������ �����̰� ����ȭ �� ����
        if (!PhotonNetwork.IsMasterClient) return;

        if (TargetPlayer != null)
            distance = Vector3.Distance(rig.transform.position, TargetPlayer.position);
        if(canMove)
        {
            StartCoroutine(Attack());
            canMove = false; 
        }
    }
    
    public void CutScene()
    {
        StartCoroutine(cutScenecoroutine());
    }

    IEnumerator cutScenecoroutine()
    {
        anim.SetTrigger("CutScene");
        Camera cam = Camera.main;
        int currentMask = cam.cullingMask;
        int PLlayer = 1 << 3;
        cam.cullingMask = currentMask & ~PLlayer;
        yield return new WaitForSeconds(1.1f);
        cutscene.Priority = 13;
        yield return new WaitForSeconds(7f);
        cutscene.Priority = 9;
        Gamemanager.instance.OnGame = true;
        cam.cullingMask = currentMask;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
    public IEnumerator Attack()
    {
        System.Random random = new System.Random();
        int state = random.Next(statelength);
        currentState = (State)state;

        //currentState = State.SkillDrop;

        StartCoroutine($"{currentState.ToString()}");
        yield return null;
    }
    public IEnumerator Targeting()
    {// ���� ����ִµ������� ���߿��ٲٱ�
        while(true)
        {
            int randomTarget = RandomNum.Range(0, 4);
            TargetPlayer = Players[randomTarget];
            yield return new WaitForSeconds(RandomNum.Range(7,15));
        }
    }

    public IEnumerator SkillRush()
    {
        Quaternion targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        photonview.RPC("Warning", RpcTarget.All, 0, true);
        anim.SetTrigger(currentState.ToString());
        yield return new WaitForSeconds(1.5f);
        photonview.RPC("Warning", RpcTarget.All, 0, false);
        Vector3 boxCenter = transform.position + transform.up*3 + transform.forward*2;
        RaycastHit[] wallhits = Physics.BoxCastAll(boxCenter, new Vector3(1f, 1f, 0.5f),
            transform.forward, transform.rotation, 0.5f, 1 << 8);

        photonview.RPC("ColliderOn", RpcTarget.All, 1, true);
        while (wallhits.Length == 0)
        {
            Move();
            yield return null;

            boxCenter = transform.position + transform.up * 3 + transform.forward*2;
            wallhits = Physics.BoxCastAll(boxCenter, new Vector3(1f, 1f, 0.5f),
                transform.forward, transform.rotation, 0.5f, 1 << 8);
        }
        photonview.RPC("ColliderOn", RpcTarget.All, 1, false);

        yield return new WaitForSeconds(2f);
        canMove = true;

    }
    public IEnumerator SkillThreat()
    {
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }
    public IEnumerator SkillEat()
    {
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }
    public IEnumerator SkillSmash()
    {
        var col = boxCollider.GetComponent<AttackCollider>().pushForce = 5;
        anim.SetTrigger(currentState.ToString());
        Quaternion targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        yield return new WaitForSeconds(1f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, true);
        yield return new WaitForSeconds(0.733f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, false);

        targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        yield return new WaitForSeconds(0.95f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, true);
        yield return new WaitForSeconds(0.817f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, false);

        photonview.RPC("Warning", RpcTarget.All, 1, true);
        targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        yield return new WaitForSeconds(1f);
        photonview.RPC("Warning", RpcTarget.All, 1, false);
        photonview.RPC("ColliderOn", RpcTarget.All, 1, true);
        yield return new WaitForSeconds(0.4f);
        photonview.RPC("ColliderOn", RpcTarget.All, 1, false);

        col = 10;
        yield return new WaitForSeconds(2f);
        canMove = true;
    }
    public IEnumerator SkillDrop()
    {
        Quaternion targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);

        anim.SetTrigger(currentState.ToString());

        yield return new WaitForSeconds(3f);
        Vector3 targetposition = TargetPlayer.position;
        targetposition.y = rig.transform.position.y;
        GetComponent<Collider>().isTrigger = true;
        rig.transform.position = targetposition;
        photonview.RPC("Warning", RpcTarget.All, 2, true);
        yield return new WaitForSeconds(1.2f);
        photonview.RPC("Warning", RpcTarget.All, 2, false);
        photonview.RPC("ColliderOn", RpcTarget.All, 2, true);
        yield return new WaitForSeconds(0.4f);
        photonview.RPC("ColliderOn", RpcTarget.All, 2, false);
        GetComponent<Collider>().isTrigger = false;

        yield return new WaitForSeconds(2f);
        canMove = true;
    }

    public void Damaged(int damage)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Die()
    {
        throw new System.NotImplementedException();
    }

    public void Hit()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        rig.transform.position += transform.forward * data.moveSpeed * Time.deltaTime;
    }

    [PunRPC]
    public void Warning(int collider, bool IsOn)
    {
        switch(collider)
        {
            case 0: boxCollider.GetComponent<AttackCollider>().warning.SetActive(IsOn); break;
            case 1: sphereCollider.GetComponent<AttackCollider>().warning.SetActive(IsOn); break;
            case 2: dropCollider.GetComponent<AttackCollider>().warning.SetActive(IsOn); break;
        }
    }

    [PunRPC]
    public void ColliderOn(int collider,bool IsOn)
    {
        switch(collider)
        {
            case 0: boxCollider.enabled = IsOn; break;
            case 1: sphereCollider.enabled = IsOn; break;
            case 2: dropCollider.enabled = IsOn; break;
        }
        if (!IsOn)
          anim.SetTrigger("Bump");
    }

}