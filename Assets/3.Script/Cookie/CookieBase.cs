using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBase : MonoBehaviour, ICookie
{
    [SerializeField]
    private CookieSO Data;
    public CookieData Cookie { get; set; }

    
    CharacterController cc;
    public Animator anim { get; private set; } // 애니메이션 클립 쿠키데이터에..?
    public float moveSpeed => Data.moveSpeed;
    public float DashCooltime { get; private set; } = 0;

    protected int maxHP;
    protected int currentHP;
    protected int ATK;
    protected int DEF;

    protected float skillCT;
    protected float ultimateCT;
    //public float DashCooltime;

    public virtual void Awake()
    {
        cc = GetComponentInParent<CharacterController>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        if(Cookie == null)
        {
            Cookie = new CookieData(Data); // todo : 이거저장데이터에서 가져와야함??
        }
        maxHP = Cookie.HP;
        currentHP = maxHP;
        ATK = Cookie.ATK;
        DEF = Cookie.DEF;
    }
    public virtual void Update()
    {
        if (DashCooltime > 0)
        {
            DashCooltime -= Time.deltaTime;
        }
    }

    public virtual IEnumerator Attack() 
    { yield return null; }
     
    public virtual void Skill() { }

    public virtual void Ultimate() { }

    public virtual IEnumerator Dash(Vector3 moveDir)
    {
        Gamemanager.instance.canMove = false;
         DashCooltime = Data.dashCT;
         anim.SetTrigger("Dash");
        
         float durtion = 0;
         while (durtion < 0.13f) // 이거 초도 clip길이만큼으로 바꿔줘야함
         {
             cc.Move(moveDir.normalized * 35f * Time.deltaTime);
             durtion += Time.deltaTime;
             yield return null;
         }
         Gamemanager.instance.canMove = true;
    }

}
