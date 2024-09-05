using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMove : MonoBehaviour
{
    public ICookie Cookie;
    public Action UltimateAction;

    float moveSpeed;
    float DashCooltime = 0;
    CharacterController cc;

    Vector3 dir = Vector3.zero;
    Vector3 rotate = Vector3.zero;


    private void Awake()
    {
        Cookie = GetComponentInChildren<ICookie>();
        cc = transform.GetComponent<CharacterController>();
        Gamemanager.instance.canMove = true;
    }

    private IEnumerator Start()
    {
        yield return null;
        moveSpeed = Cookie.moveSpeed;
    }

    private void Update()
    {
        if (Cookie == null)
            return;

        //dir.y += Physics.gravity.y * Time.deltaTime;
        cc.Move(cc.transform.up * Physics.gravity.y * Time.deltaTime);
        if (Gamemanager.instance.canMove)
        {
            // 이부분은?
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotate);
        }  
    }


    void OnMove(InputValue value)
    {
        Vector3 input = -value.Get<Vector2>();

        dir = new Vector3(input.x, 0, input.y);

        if (dir != Vector3.zero)
        {
            rotate = new Vector3(0, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, 0);
            Cookie.anim.SetBool("IsMove", true);
        }
        else
            Cookie.anim.SetBool("IsMove", false);
    }

    void OnAttack()
    {
        if(Gamemanager.instance.canMove)
           StartCoroutine(Cookie.Attack());
    }

    void OnSkill()
    {
        if (Gamemanager.instance.canMove)
            StartCoroutine(Cookie.Skill());
    }
    IEnumerator OnUltimate()
    {
        if (Gamemanager.instance.canMove && Cookie.UltimateCT <= 0)
        {
            StartCoroutine(Cookie.Ultimate());
            yield return new WaitForSeconds(0.1f);
            UltimateAction?.Invoke();
        }
    }
    void OnDash()
    {
        if (Cookie.DashCT <= 0)
        {
            transform.rotation = Quaternion.Euler(rotate);

            Quaternion rotation = Quaternion.Euler(rotate);
            Vector3 moveDir = rotation * Vector3.forward;

            StartCoroutine(Cookie.Dash(moveDir));            
        }
    }
    
}
