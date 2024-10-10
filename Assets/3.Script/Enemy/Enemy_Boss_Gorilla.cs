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
    public int CurrentHP;
    int DEF;
    public float HPPer => (float)CurrentHP / data.HP;

    Animator anim;
    public bool canMove = false;
    float atkCT;
    int statelength;
    public Transform transform;
    public List<Transform> Players = new List<Transform>();
    public Transform TargetPlayer;
    public CinemachineVirtualCamera cutscene;
    public BoxCollider boxCollider;
    public SphereCollider sphereCollider;
    public SphereCollider dropCollider;
    State currentState;

    public bool isdie;
    Rigidbody rig;
    PhotonView photonview;
    BossHPPanel HPPanel;
    AudioSource sound;

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
        dropCollider.GetComponent<AttackCollider>().ATK = (int)(data.ATK * 2);
        HPPanel = HPBarPanel.instance.gameObject.GetComponentInChildren<BossHPPanel>(true);
        CurrentHP = data.HP;
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isdie || !RaidManager.instance.isPlay) return;
        if (CurrentHP <= 0)
        {
            StopAllCoroutines();
            GetComponent<Collider>().enabled = false;
            StartCoroutine(Die());
            isdie = true;
        }

        // 마스터 클라이언트에서만 움직이고 동기화 할 예정
        if (!PhotonNetwork.IsMasterClient) return;

        if (TargetPlayer == null)
        {
            int randomTarget = RandomNum.Range(0, Players.Count);
            TargetPlayer = Players[randomTarget];
        }

        if(canMove)
        {
            StartCoroutine(Attack());
            canMove = false; 
        }

        if (TargetPlayer.GetComponent<PlayerRPC>().isDie)
        {
            Players.Remove(TargetPlayer);
            int randomTarget = RandomNum.Range(0, Players.Count);
            TargetPlayer = Players[randomTarget];
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
    {// 조건 살아있는동안으로 나중에바꾸기
        while(true)
        {
            int randomTarget = RandomNum.Range(0, Players.Count);
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
        soundPlay(data.RushStart);
        yield return new WaitForSeconds(1.5f);
        photonview.RPC("Warning", RpcTarget.All, 0, false);
        Vector3 boxCenter = transform.position + transform.up*3 + transform.forward*2;
        RaycastHit[] wallhits = Physics.BoxCastAll(boxCenter, new Vector3(1f, 1f, 0.5f),
            transform.forward, transform.rotation, 0.5f, 1 << 8);

        photonview.RPC("ColliderOn", RpcTarget.All, 1, true);
        sound.loop = true;
        soundPlay(data.RushLoop);
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
        soundPlay(data.ATK1);
        yield return new WaitForSeconds(0.733f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, false);

        targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        yield return new WaitForSeconds(0.95f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, true);
        soundPlay(data.ATK2);
        yield return new WaitForSeconds(0.817f);
        photonview.RPC("ColliderOn", RpcTarget.All, 0, false);

        photonview.RPC("Warning", RpcTarget.All, 1, true);
        targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        rig.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        yield return new WaitForSeconds(1f);
        photonview.RPC("Warning", RpcTarget.All, 1, false);
        photonview.RPC("ColliderOn", RpcTarget.All, 1, true);
        soundPlay(data.ATK3);
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
        GetComponent<Collider>().isTrigger = true;
        Vector3 targetposition = TargetPlayer.position;
        targetposition.y = rig.transform.position.y;
        rig.transform.position = targetposition;
        photonview.RPC("Warning", RpcTarget.All, 2, true);
        yield return new WaitForSeconds(1.2f);
        photonview.RPC("Warning", RpcTarget.All, 2, false);
        photonview.RPC("ColliderOn", RpcTarget.All, 2, true);
        soundPlay(data.Drop);
        yield return new WaitForSeconds(0.4f);
        photonview.RPC("ColliderOn", RpcTarget.All, 2, false);
        GetComponent<Collider>().isTrigger = false;

        yield return new WaitForSeconds(2f);
        canMove = true;
    }

    public void Damaged(int damage)
    {
        photonview.RPC("DamagedRPC", RpcTarget.All, damage);
        RaidManager.instance.Damage += damage;
    }

    public IEnumerator Die()
    {
        anim.SetTrigger("Die");
        RaidManager.instance.ClearGame = true;
        StartCoroutine(RaidManager.instance.EndGame());
        yield return null;
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
        {
            sound.loop = false;
            soundPlay(data.RushEnd);
            anim.SetTrigger("Bump");
        }
    }

    [PunRPC]
    public void DamagedRPC(int damage)
    {
        CurrentHP -= damage;
        HPPanel.Refresh(HPPer);
        HPBarPanel.instance.DamageText(this.transform, damage);
    }

    public void soundPlay(AudioClip clip)
    {
        sound.Stop();
        sound.clip = clip;
        sound.Play();
    }

}
