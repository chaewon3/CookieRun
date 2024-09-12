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

    public int ATK { get; set; }
    protected int HP;
    protected int DEF;

    protected bool canMove;
    protected float atkCT;

    public float HPPer => (float)HP / data.HP;


    protected float distance;

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        HP = data.HP;
        ATK = data.ATK;
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
        if (!Stagemanager.instance.onGame)
        {
            StopAllCoroutines();
            anim.SetTrigger("Idle");
            return;
        }

        distance = Vector3.Distance(rig.transform.position, target.transform.position);
        
        if (atkCT > 0)
            atkCT -= Time.deltaTime;
        else
            atkCT = 0;
    }

    public virtual void LateUpdate()
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
        rig.transform.position += rig.transform.forward * data.moveSpeed * Time.deltaTime;
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }

    public virtual IEnumerator Die()
    {
        anim.SetTrigger("Die");

        //yield return new WaitForSeconds(0.3f);

        GameObject coin = Resources.Load<GameObject>("MonsterCoin");
        if(coin!=null)
        {
            DropCoin(coin);
            DropCoin(coin);
            DropCoin(coin);
        }

        float durtion = 0;
        while (durtion < 0.13f) // 이거 초도 clip길이만큼으로 바꿔줘야함
        {
            rig.transform.position += -rig.transform.forward * 8 * Time.deltaTime;
            durtion += Time.deltaTime;
            yield return null;
        }
        if(Area != null)
          Area.enemyDie(this.gameObject);
        HPBarPanel.instance.RemoveHPPanel(this.transform);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    public virtual void Damaged(int damage)
    {
        if (HP <= 0)
            return;
        if (HP == data.HP)
            HPBarPanel.instance.SetHPPanel(this.transform);

        //anim.SetTrigger("Damaged");
        HP -= damage;

        HPBarPanel.instance.RefreshHP(this.transform, damage);

        if (HP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    public virtual void Hit()
    {

    }

    void DropCoin(GameObject coinprefab)
    {
        GameObject coin = Instantiate(coinprefab,transform.position+Vector3.up*0.3f,Quaternion.identity, transform);
        coin.transform.SetParent(null);
        coin.GetComponentInChildren<Coin>().coin = data.coin;
        coin.transform.Rotate(0,Random.Range(1,360),0);

        coin.GetComponent<Rigidbody>().AddForce(coin.transform.forward*2,ForceMode.Impulse);
    }
}
