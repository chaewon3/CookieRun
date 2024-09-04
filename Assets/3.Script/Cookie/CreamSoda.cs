using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamSoda : CookieBase
{
    int comboCount = 0;
    float comboTime;

    public override void Update()
    {
        base.Update();

        if (comboTime > 0)
            comboTime -= Time.deltaTime;
        else if (comboTime <= 0)
            comboCount = 0;
    }

    public override IEnumerator Attack()
    {
        //todo : 나중에 애니메이션 클립으로 넣어야함
        // todo : PC일 경우에 마우스 방향 타겟팅 / 모바일일 경우에 오토타겟팅
        if (Gamemanager.instance.canMove && comboCount == 0)
        {
            Gamemanager.instance.canMove = false;
            comboCount++;
            comboTime = 0.95f;
            StartCoroutine(attackDash(5f, 0.1f));
            anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(0.4f);
            Gamemanager.instance.canMove = true;
        }
        else if (Gamemanager.instance.canMove && comboCount == 1 && comboTime > 0)
        {
            Gamemanager.instance.canMove = false;
            comboCount++;
            comboTime = 1.15f;
            StartCoroutine(attackDash(5f, 0.1f));
            anim.SetTrigger("Attack2");
            yield return new WaitForSeconds(0.642f);
            Gamemanager.instance.canMove = true;
        }
        else if (Gamemanager.instance.canMove && comboCount == 2 && comboTime > 0)
        {
            Gamemanager.instance.canMove = false;
            comboCount = 0;
            StartCoroutine(attackDash(10f, 0.1f));
            anim.SetTrigger("Attack3");
            yield return new WaitForSeconds(0.433f);
            Gamemanager.instance.canMove = true;
        }
    }

    public override void Skill()
    {
        base.Skill();
    }

    public override void Ultimate()
    {
        base.Ultimate();
    }
    public override IEnumerator Dash(Vector3 moveDir)
    {
        yield return StartCoroutine(base.Dash(moveDir));
    }

    IEnumerator attackDash(float dir, float time)
    {
        yield return null;
    }
}
