using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    CookieBase Cookie;

    float moveSpeed;
    bool canMove = true; //todo : ���߿� �����Ҷ� ���������
    float DashCooltime = 0;
    CharacterController cc;
    Animator anim;

    int comboCount = 0;
    float comboTime;
    Vector3 dir = Vector3.zero;
    Vector3 rotate = Vector3.zero;

    private void Awake()
    {
        Cookie = GetComponentInChildren<CookieBase>();
        moveSpeed = Cookie.Cookie.Data.moveSpeed;
    }

    private void Update()
    {
        if (Cookie == null)
            return;

        dir.y += Physics.gravity.y * Time.deltaTime;
        if (canMove)
        {
            // �̺κ���?
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotate);
        }

        //// �̺κ��� ��°�� ��Ű������Ʈ�� �ֱ�
        //if (comboTime > 0)
        //    comboTime -= Time.deltaTime;
        //else if (comboTime <= 0)
        //    comboCount = 0;

        //if(DashCooltime >0)
        //{
        //    DashCooltime -= Time.deltaTime;
        //}
    }


    void OnMove(InputValue value)
    {
        Vector3 input = -value.Get<Vector2>();

        dir = new Vector3(input.x, 0, input.y);

        if (dir != Vector3.zero)
        {
            rotate = new Vector3(0, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, 0);
            anim.SetBool("IsMove", true);
        }
        else
            anim.SetBool("IsMove", false);
    }

    void OnAttack()
    {
        Cookie.Attack();
        //todo : ���߿� �ִϸ��̼� Ŭ������ �־����
        // todo : PC�� ��쿡 ���콺 ���� Ÿ���� / ������� ��쿡 ����Ÿ����
        //if(canMove && comboCount == 0)
        //{
        //    canMove = false;
        //    comboCount++;
        //    comboTime = 0.95f;
        //    StartCoroutine(Dash(5f, 0.1f));
        //    anim.SetTrigger("Attack1");
        //    yield return new WaitForSeconds(0.4f);
        //    canMove = true;
        //}
        //else if (canMove && comboCount == 1 && comboTime > 0)
        //{
        //    canMove = false;
        //    comboCount++;
        //    comboTime = 1.15f;
        //    StartCoroutine(Dash(5f, 0.1f));
        //    anim.SetTrigger("Attack2");
        //    yield return new WaitForSeconds(0.642f);
        //    canMove = true;
        //}
        //else if (canMove && comboCount == 2 && comboTime > 0)
        //{
        //    canMove = false;
        //    comboCount = 0;
        //    StartCoroutine(Dash(10f,0.1f));
        //    anim.SetTrigger("Attack3");
        //    yield return new WaitForSeconds(0.433f);
        //    canMove = true;
        //}
    }

    IEnumerator OnDash()
    {
        if (DashCooltime <= 0)
        {
            canMove = false;
            transform.rotation = Quaternion.Euler(rotate);

            Quaternion rotation = Quaternion.Euler(rotate);
            Vector3 moveDir = rotation * Vector3.forward;

            Cookie.Dash(moveDir);
            yield return new WaitForSeconds(0.13f);
            canMove = true;
        }
    }
    
}
