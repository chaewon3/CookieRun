using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemy
{
    public EnemySO data;

    protected GameObject target;

    protected Rigidbody rig;
    protected Animator anim;

    protected int HP;
    protected int ATk;
    protected int DEF;

    protected bool canMove;
    protected float atkCT;

    public int HPPer => HP/data.HP;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        HP = data.HP;
        ATk = data.ATK;
        DEF = data.DEF;
        target = FindObjectOfType<PlayerMove>().gameObject;
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.67f); // 나중에 spawn애니메이션 추가하고 시간 바까야함
        canMove = true;
    }

    public virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        if (distance > 1 && canMove)
            Move();
        else if( distance <= 1 && canMove)
        {
            anim.SetBool("Move", false);
            StartCoroutine(Attack());
        }

        if (HP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }

        if (atkCT > 0)
            atkCT -= Time.deltaTime;
        else
            atkCT = 0;
    }

    private void LateUpdate()
    {
        if(canMove)
            transform.LookAt(target.transform);
    }

    public void Move()
    {
        anim.SetBool("Move", true);
        transform.position += transform.forward * data.moveSpeed * Time.deltaTime;
    }

    public virtual IEnumerator Attack()
    {
        if(atkCT == 0)
        {
            canMove = false;
            atkCT = 2;
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1.93f);
            canMove = true;
        }
    }

    public virtual IEnumerator Die()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        HPBarPanel.instance.RemoveHPPanel(this.transform);
        Destroy(this.gameObject);
    }

    public virtual void Hit(int damage, float nuckback)
    {
        if (HP == data.HP)
            HPBarPanel.instance.SetHPPanel(this.transform);

        anim.SetTrigger("Damaged");
        HP -= damage;
    }

    
}
