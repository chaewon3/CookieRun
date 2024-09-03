using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBase : MonoBehaviour, ICookie
{
    [SerializeField]
    private CookieSO Data;
    public CookieData Cookie { get; set; }

    
    CharacterController cc;
    public Animator anim; // �ִϸ��̼� Ŭ�� ��Ű�����Ϳ�..?

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
            Cookie = new CookieData(Data); // todo : �̰����嵥���Ϳ��� �����;���??
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
            while (durtion < 0.13f) // �̰� �ʵ� clip���̸�ŭ���� �ٲ������
            {
                cc.Move(moveDir.normalized * 35f * Time.deltaTime);
                durtion += Time.deltaTime;
                yield return null;
            }
        }        
    }


}
