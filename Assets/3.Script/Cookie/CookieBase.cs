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
    protected Animator damagedVolume;

    protected bool crashed;
    protected bool isDie;

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
        maxHP = Cookie.HP;
        CurrentHP = maxHP;
        ATK = Cookie.ATK;
        DEF = Cookie.DEF;
        cc = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
        RPCHP();
        damagedVolume = GameObject.Find("Damaged").GetComponent<Animator>();
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
        CurrentHP -= damage;
        damagedVolume.SetTrigger("Damaged");
        if (CurrentHP <=0)
        {
            CurrentHP = 0;
            Gamemanager.instance.OnGame = false;
            Gamemanager.instance.canMove = false;
            anim.SetTrigger("Die");
            StartCoroutine(CreateGhost());
            Gamemanager.instance.IsDie = true;
        }
        RPCHP();
    }

    IEnumerator CreateGhost()
    {
        crashed = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        if (PhotonNetwork.InRoom)
            RaidManager.instance.CreateGhost();
        Destroy(gameObject);
    }
    public IEnumerator Crashed(Vector3 direction)
    {
        if (crashed) yield break;
        crashed = true;
        float time = 0.3f;
        while (time >= 0)
        {
            _ = cc.Move(direction * Time.deltaTime);
            yield return null;
            time -= Time.deltaTime;
        }
        crashed = false;
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
