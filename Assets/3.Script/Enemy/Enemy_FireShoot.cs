using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FireShoot : EnemyBase
{
    GameObject FireArrowPrefab;
    public Transform ShootPoint;

    public override void Awake()
    {
        base.Awake();
        FireArrowPrefab = data.ModelPrefab;
    }

    public override void Update()
    {
        base.Update();
        if(canMove&&distance < 10)
        {
            StartCoroutine(Attack());
        }
    }

    public override IEnumerator Attack()
    {
        if (atkCT == 0)
        {
            atkCT = 5f;
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(0.467f);
            GameObject Arrow = Instantiate(FireArrowPrefab, ShootPoint.position+Vector3.up*0.5f, transform.rotation, transform);
            yield return new WaitForSeconds(0.5f);
            canMove = false;
            Arrow.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            canMove = true;
        }
    }
}
