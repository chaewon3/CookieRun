using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieBase : MonoBehaviour, ICookie
{
    [SerializeField]
    protected CookieSO Data;
    CookieData Cookie { get; set; }
        
    protected CharacterController cc;
    public Animator anim { get; protected set; } // �ִϸ��̼� Ŭ�� ��Ű�����Ϳ�..?
    public float moveSpeed => Data.moveSpeed;
    public float DashCT { get; protected set; } = 0;
    public float DashCoolTimeFillAmount => DashCT/Data.dashCT;
    public float SkillCT { get; protected set; } = 0;
    public float SkillCoolTimeFillAmount => SkillCT/Data.skillCT;
    public float UltimateCT { get; protected set; } = 0;
    public float UltimateCoolTimeFillAmount => UltimateCT/Data.ultimateCT;
    public Sprite CutSceneIcon => Data.icon;
    public int CurrentHP { get; set; }
    public float HPPer => (float)CurrentHP/maxHP;

    protected int maxHP;
    protected int ATK;
    protected int DEF;

    public virtual void Awake()
    {
        cc = GetComponentInParent<CharacterController>();
        GameObject cookie = Instantiate(Data.ModelPrefab, transform.position, transform.rotation, transform);
        anim = GetComponentInChildren<Animator>();       
    }
    private void Start()
    {
        if(Cookie == null)
        {
            Cookie = new CookieData(Data); // todo : �̰����嵥���Ϳ��� �����;���??
        }
        maxHP = Data.baseHP;
        CurrentHP = maxHP;
        ATK = Cookie.ATK;
        DEF = Cookie.DEF;
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
        Gamemanager.instance.canMove = false;
         DashCT = Data.dashCT;
         anim.SetTrigger("Dash");
        
         float durtion = 0;
         while (durtion < 0.13f) // �̰� �ʵ� clip���̸�ŭ���� �ٲ������
         {
             cc.Move(moveDir.normalized * 25f * Time.deltaTime);
             durtion += Time.deltaTime;
             yield return null;
         }
         Gamemanager.instance.canMove = true;
    }

    public void Hit(int damage)
    {
        CurrentHP -= (damage);
    }
}
