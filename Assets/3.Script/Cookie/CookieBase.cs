using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieBase : MonoBehaviour, ICookie
{
    CharacterController cc;
    public Animator anim;
    // 애니메이션 클립들은 상속할 함수에

    public int currentHP;
    public int maxHP;
    public int ATK;
    public int DEF;

    public float skillCT;
    public float ultimateCT;
    public float moveSpeed;

    public float DashCooltime;    

    public virtual void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (DashCooltime > 0)
        {
            DashCooltime -= Time.deltaTime;
        }
    }

    public virtual void Attack()
    {

    }

    public virtual void Dash(Vector3 moveDir)
    {
        StartCoroutine(dash(moveDir));
    }

    public virtual void Skill()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Ultimate()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator dash(Vector3 moveDir)
    {
        if (DashCooltime <= 0)
        {
            DashCooltime = 3f;
            anim.SetTrigger("Dash");

            float durtion = 0;
            while (durtion < 0.13f)
            {
                cc.Move(moveDir.normalized * 35f * Time.deltaTime);
                durtion += Time.deltaTime;
                yield return null;
            }
        }        
    }


}
