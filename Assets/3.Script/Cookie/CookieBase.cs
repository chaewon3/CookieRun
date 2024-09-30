using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CookieBase : MonoBehaviour, ICookie
{
    protected CookieSO Data;
    public CookieData Cookie { get; set; }
        
    protected CharacterController cc;
    public Animator anim { get; protected set; } // 애니메이션 클립 쿠키데이터에..?
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
         while (durtion < 0.13f) // 이거 초도 clip길이만큼으로 바꿔줘야함
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
    }
}
