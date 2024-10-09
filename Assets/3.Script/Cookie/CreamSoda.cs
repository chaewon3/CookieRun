using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamSoda : CookieBase
{
    int comboCount = 0;
    int skillCount = 0;
    float comboTime;
    float skillTime;

    Vector3 attactrange = new Vector3(0.7f,0.7f,0.7f);
    Vector3 skillrange = new Vector3(1,1,1);
    Vector3 ultimaterange = new Vector3(1, 1, 1);

    public override void Update()
    {
        base.Update();

        if (comboTime > 0)
            comboTime -= Time.deltaTime;
        else
            comboCount = 0;

        if (SkillCT > 0)
            SkillCT -= Time.deltaTime;
        else
        {
            SkillCT = 0;
            skillTime = 0;
        }

        if (skillTime > 0)
            skillTime -= Time.deltaTime;
        else
            skillTime = 0;

        if (UltimateCT > 0)
            UltimateCT -= Time.deltaTime;
        else
            UltimateCT = 0;

    }

    public override IEnumerator Attack()
    {
        //todo : 나중에 애니메이션 클립으로 넣어야함
        // todo : PC일 경우에 마우스 방향 타겟팅 / 모바일일 경우에 오토타겟팅
        Gamemanager.instance.canMove = false;
        if (comboCount == 0)
        {
            soundPlay(Data.ATKclip1);
            comboCount++;
            comboTime = 0.95f;
            StartCoroutine(attackDash(5f, 0.1f));
            StartCoroutine(Raycast(0.1f, attactrange, ATK*0.76f,0.1f));
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.4f);
        }
        else if (comboCount == 1 && comboTime > 0)
        {
            soundPlay(Data.ATKclip2);
            comboCount++;
            comboTime = 1.15f;
            StartCoroutine(attackDash(5f, 0.1f));
            StartCoroutine(Raycast(0.1f, attactrange, ATK * 0.76f, 0.1f));
            anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(0.4f);
        }
        else if (comboCount == 2 && comboTime > 0)
        {
            soundPlay(Data.ATKclip3);
            comboCount = 0;
            StartCoroutine(attackDash(10f, 0.1f));
            StartCoroutine(Raycast(0.1f, attactrange, ATK, 0.1f));
            anim.SetTrigger("Attack3");
            yield return new WaitForSeconds(0.433f);
        }
        Gamemanager.instance.canMove = true;
    }

    public override IEnumerator Skill()
    {
        anim.SetBool("Move", false);
        Gamemanager.instance.canMove = false;
        if (SkillCT <= 0)
        {
            soundPlay(Data.skillclip1);
            skillCount++;
            SkillCT = Data.skillCT;
            skillTime = 4;
            anim.SetTrigger("Skill1");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(20f, 0.1f));
            StartCoroutine(Raycast(1, skillrange, ATK * 2.39f, 0.1f));
            yield return new WaitForSeconds(0.417f);
        }
        else if (skillCount == 1 && skillTime > 0)
        {
            soundPlay(Data.skillclip2);
            skillCount++;
            skillTime = 4;
            anim.SetTrigger("Skill2");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(20f, 0.1f));
            StartCoroutine(Raycast(1, skillrange, ATK * 2.47f, 0.1f));
            yield return new WaitForSeconds(0.383f);
        }
        else if (skillCount == 2 && skillTime > 0)
        {
            soundPlay(Data.skillclip3);
            skillCount = 0;
            anim.SetTrigger("Skill3");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(20f, 0.1f));
            StartCoroutine(Raycast(1, skillrange, ATK * 2.64f, 0.1f));
            yield return new WaitForSeconds(0.583f);
        }
        Gamemanager.instance.canMove = true;
        anim.SetBool("Move", true);
    }

    public override IEnumerator Ultimate()
    {
        soundPlay(Data.ultiamteclip);
        anim.SetBool("Move", false);
        Gamemanager.instance.canMove = false;
        UltimateCT = Data.ultimateCT;
        anim.SetTrigger("Ultimate");
        StartCoroutine(Raycast(2, ultimaterange, ATK * 4.9f, 1.2f));
        StartCoroutine(Raycast(2, ultimaterange, ATK * 4.9f, 1.3f));
        StartCoroutine(Raycast(2, ultimaterange, ATK * 4.9f, 1.4f));
        yield return new WaitForSeconds(1.75f);
        Gamemanager.instance.canMove = true;
        anim.SetBool("Move", true);
    }
    public override IEnumerator Dash(Vector3 moveDir)
    {
        yield return StartCoroutine(base.Dash(moveDir));
    }

    IEnumerator attackDash(float dis, float time)
    {
        float durtion = 0;

        while (durtion < time)
        {
            cc.Move(cc.transform.forward * dis * Time.deltaTime);
            durtion += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Raycast(float range,Vector3 boxsize, float damage, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Vector3 boxCenter = cc.transform.position +cc.transform.up + cc.transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxsize, cc.transform.forward, Quaternion.identity, range);

        LayerMask enemylayer = LayerMask.NameToLayer("Enemy");
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer != enemylayer)
            {
                continue;
            }
            if(hit.collider.TryGetComponent<IEnemy>(out IEnemy enemy))
            {
                enemy.Damaged((int)damage);
            }
        }
    }

}
