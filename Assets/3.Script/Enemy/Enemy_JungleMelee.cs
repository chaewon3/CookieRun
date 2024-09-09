using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_JungleMelee : EnemyBase
{
    public override void Update()
    {
        base.Update();
        if (distance > 1 && canMove)
            Move();
        else if (distance <= 1 && canMove)
        {
            anim.SetBool("Move", false);
            StartCoroutine(Attack());
        }
    }
    public override IEnumerator Attack()
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

    public override void Hit()
    {
        Vector3 boxCenter = transform.position + transform.up + transform.forward;
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, new Vector3(0.5f, 0.5f, 0.5f),
            transform.forward, Quaternion.identity, 0.5f);

        LayerMask targetlayer = LayerMask.NameToLayer("Player");
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer != targetlayer)
            {
                continue;
            }

            if (hit.collider.TryGetComponent<PlayerMove>(out PlayerMove cookie))
            {
                cookie.Cookie.Hit((int)ATK);
            }
        }
    }
}
