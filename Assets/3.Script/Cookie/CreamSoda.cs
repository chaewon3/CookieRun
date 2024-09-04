using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamSoda : CookieBase
{
    int comboCount = 0;
    int skillCount = 0;
    float comboTime;
    float skillTime;

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
        //todo : ���߿� �ִϸ��̼� Ŭ������ �־����
        // todo : PC�� ��쿡 ���콺 ���� Ÿ���� / ������� ��쿡 ����Ÿ����
        Gamemanager.instance.canMove = false;
        if (comboCount == 0)
        {
            comboCount++;
            comboTime = 0.95f;
            StartCoroutine(attackDash(5f, 0.1f));
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.4f);
        }
        else if (comboCount == 1 && comboTime > 0)
        {
            comboCount++;
            comboTime = 1.15f;
            StartCoroutine(attackDash(5f, 0.1f));
            anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(0.642f);
        }
        else if (comboCount == 2 && comboTime > 0)
        {
            comboCount = 0;
            StartCoroutine(attackDash(10f, 0.1f));
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
            skillCount++;
            SkillCT = Data.skillCT;
            skillTime = 4;
            anim.SetTrigger("Skill1");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(30f, 0.1f));
            yield return new WaitForSeconds(0.417f);
        }
        else if (skillCount == 1 && skillTime > 0)
        {
            skillCount++;
            skillTime = 4;
            anim.SetTrigger("Skill2");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(30f, 0.1f));
            yield return new WaitForSeconds(0.383f);
        }
        else if (skillCount == 2 && skillTime > 0)
        {
            skillCount = 0;
            anim.SetTrigger("Skill3");
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(attackDash(30f, 0.1f));
            yield return new WaitForSeconds(0.583f);
        }
        Gamemanager.instance.canMove = true;
        anim.SetBool("Move", true);
    }

    public override IEnumerator Ultimate()
    {
        anim.SetBool("Move", false);
        Gamemanager.instance.canMove = false;
        UltimateCT = Data.ultimateCT;
        anim.SetTrigger("Ultimate");
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
}
