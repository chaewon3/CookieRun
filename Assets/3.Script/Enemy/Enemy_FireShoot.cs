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
            canMove = false;
            atkCT = 4f;
            anim.SetTrigger("Attack");
            GameObject Arrow = Instantiate(FireArrowPrefab, ShootPoint.position+Vector3.up, transform.rotation, transform);
            Quaternion targetrotation = Quaternion.LookRotation(target.transform.position - Arrow.transform.position);
            Arrow.transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);
            Arrow.SetActive(false);
            yield return new WaitForSeconds(1f);
            Arrow.SetActive(true);
            canMove = true;
        }
    }
}
