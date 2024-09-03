using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBase : MonoBehaviour, ICookie
{
    [SerializeField]
    private CookieSO Data;
    public CookieData Cookie { get; set; }

    
    CharacterController cc;
    public Animator anim; // 애니메이션 클립 쿠키데이터에..?

    public int maxHP;
    public int currentHP;
    public int ATK;
    public int DEF;

    public float skillCT;
    public float ultimateCT;
    public float DashCooltime;

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

    public virtual void Attack() { }

    public virtual void Dash(Vector3 moveDir)
    {
        StartCoroutine(dash(moveDir));
    }

    public virtual void Skill() { }

    public virtual void Ultimate() { }

    IEnumerator dash(Vector3 moveDir)
    {
        if (DashCooltime <= 0)
        {
            DashCooltime = Data.dashCT;
            anim.SetTrigger("Dash");

            float durtion = 0;
            while (durtion < 0.13f) // 이거 초도 clip길이만큼으로 바꿔줘야함
            {
                cc.Move(moveDir.normalized * 35f * Time.deltaTime);
                durtion += Time.deltaTime;
                yield return null;
            }
        }        
    }


}
