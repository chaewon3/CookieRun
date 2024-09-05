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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        HP = data.HP;
        ATk = data.ATK;
        DEF = data.DEF;
        target = FindObjectOfType<PlayerMove>().gameObject;
    }

    public virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 1)
            Move();
        else
        {
            anim.SetBool("Move", false);
            Attack();
        }

        if (HP <= 0)
            Die();

        if (atkCT > 0)
            atkCT -= Time.deltaTime;
        else
            atkCT = 0;
    }

    private void LateUpdate()
    {
        transform.LookAt(target.transform);
    }

    public void Move()
    {
        anim.SetBool("Move", true);
        transform.position += transform.forward * data.moveSpeed * Time.deltaTime;
    }

    public virtual void Attack()
    {
        if(atkCT == 0)
        {
            atkCT = 2;
            anim.SetTrigger("Attack");
        }
    }

    public virtual void Die()
    {
        anim.SetTrigger("Die");
        Destroy(this.gameObject);
    }

    public virtual void Hit(int damage)
    {
        anim.SetTrigger("Damaged");
        HP -= damage;
    }

    
}
