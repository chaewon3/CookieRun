using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemy
{
    public EnemySO data;
    public GameObject SpawnEffect;
    public SpawnArea Area;

    protected GameObject target;

    protected Rigidbody rig;
    protected Animator anim;

    protected int HP;
    protected int ATk;
    protected int DEF;

    protected bool canMove;
    protected float atkCT;

    public float HPPer => (float)HP / data.HP;

    float distance;

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
        yield return new WaitForSeconds(2f); // 나중에 spawn애니메이션 추가하고 시간 바까야함
        canMove = true;
        SpawnEffect.SetActive(false);
    }


    public virtual void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 1 && canMove)
            Move();
        else if (distance <= 1 && canMove)
        {
            anim.SetBool("Move", false);
            StartCoroutine(Attack());
        }

        if (atkCT > 0)
            atkCT -= Time.deltaTime;
        else
            atkCT = 0;
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            Quaternion targetrotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
        }
    }

    public void Move()
    {
        anim.SetBool("Move", true);
        transform.position += transform.forward * data.moveSpeed * Time.deltaTime;
    }

    public virtual IEnumerator Attack()
    {
        if (atkCT == 0)
        {
            canMove = false;
            atkCT = 2;
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(1.133f);
            Hit();
            yield return new WaitForSeconds(1.93f);
            canMove = true;
        }
    }

    public virtual IEnumerator Die()
    {
        anim.SetTrigger("Die");

        float durtion = 0;
        while (durtion < 0.13f) // 이거 초도 clip길이만큼으로 바꿔줘야함
        {
            transform.position += -transform.forward * 8 * Time.deltaTime;
            durtion += Time.deltaTime;
            yield return null;
        }
        Area.enemyDie(this.gameObject);
        HPBarPanel.instance.RemoveHPPanel(this.transform);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    public virtual void Hit(int damage, float nuckback = 0)
    {
        if (HP <= 0)
            return;
        if (HP == data.HP)
            HPBarPanel.instance.SetHPPanel(this.transform);

        //anim.SetTrigger("Damaged");
        HP -= damage;
        HPBarPanel.instance.RefreshHP(this.transform);

        if (HP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    void Hit()
    {
        Vector3 boxCenter = transform.position + transform.up + transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, new Vector3(0.5f,0.5f,0.5f),
            transform.forward, Quaternion.identity, 0.5f);

        LayerMask targetlayer = LayerMask.NameToLayer("Player");
        foreach (RaycastHit hit in hits)
        {
            print(hit.transform.name);
            if (hit.collider.gameObject.layer != targetlayer)
            {
                continue;
            }

            if (hit.collider.TryGetComponent<PlayerMove>(out PlayerMove cookie))
            {
                cookie.Cookie.Hit((int)ATk);
                print((int)ATk);
            }

        }
    }
}
