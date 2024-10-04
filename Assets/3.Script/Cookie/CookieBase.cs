using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CookieBase : MonoBehaviour, ICookie
{
    protected CookieSO Data;
    public CookieData Cookie { get; set; }
        
    protected CharacterController cc;
    public Animator anim { get; protected set; } // �ִϸ��̼� Ŭ�� ��Ű�����Ϳ�..?
    public float moveSpeed => Data.moveSpeed;
    public float DashCT { get; protected set; } = 0;
    public float DashCoolTimeFillAmount => DashCT/Data.dashCT;
    public float SkillCT { get; protected set; } = 0;
    public float SkillCoolTimeFillAmount => SkillCT/Data.skillCT;
    public float UltimateCT { get; protected set; } = 0;
    public float UltimateCoolTimeFillAmount => UltimateCT/Data.ultimateCT;
    public Sprite CutSceneIcon => Data.portrait;
    public int CurrentHP { get; set; }
    public float HPPer => (float)CurrentHP/maxHP;

    protected int maxHP;
    protected int ATK;
    protected int DEF;
    protected PhotonView photonView;
    public virtual void Awake()
    {
        if(PhotonNetwork.IsConnected)
        {
            if (!(photonView = GetComponent<PhotonView>()).IsMine)
                this.GetComponent<CookieBase>().enabled = false;
        }
        
    }
    private void Start()
    {
        Data = Cookie.Data;
        maxHP = Data.baseHP;
        CurrentHP = maxHP;
        ATK = Cookie.ATK;
        DEF = Cookie.DEF;
        cc = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        RPCHP();
    }

    public virtual void Update()
    {
        if (DashCT > 0)
        {
            DashCT -= Time.deltaTime;
        }
    }

    public virtual IEnumerator Attack() { yield return null; }
     
    public virtual IEnumerator Skill() { yield return null; }

    public virtual IEnumerator Ultimate() { yield return null; }

    public virtual IEnumerator Dash(Vector3 moveDir)
    {
         DashCT = Data.dashCT;
         anim.SetTrigger("Dash");
        
         float durtion = 0;
         while (durtion < 0.13f) // �̰� �ʵ� clip���̸�ŭ���� �ٲ������
         {
             cc.Move(moveDir.normalized * 15f * Time.deltaTime);
             durtion += Time.deltaTime;
             yield return null;
         }
    }

    public void Hit(int damage)
    {
        CurrentHP -= (damage);
        if(CurrentHP <=0)
        {
            CurrentHP = 0;
            anim.SetTrigger("Die");
            Stagemanager.instance.onGame = false;
            Gamemanager.instance.canMove = false;
        }
        RPCHP();
    }

    void RPCHP()
    {
        if (TryGetComponent<PhotonView>(out PhotonView photonview))
        {
            if (PhotonNetwork.InRoom && photonview.Owner == PhotonNetwork.LocalPlayer)
            {
                Player localplayer = PhotonNetwork.LocalPlayer;
                Hashtable HP = new Hashtable();
                HP["HP"] = CurrentHP;
                localplayer.SetCustomProperties(HP);

                Hashtable HPPer = new Hashtable();
                HPPer["HPPer"] = this.HPPer;
                localplayer.SetCustomProperties(HPPer);
            }
        }
    }
}
