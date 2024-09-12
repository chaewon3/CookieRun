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
    {        //todo :나중에 쿠키태그시스템 생기면 직접 넣어줘야함
        Cookie = GetComponentInChildren<ICookie>();
        cc = transform.GetComponent<CharacterController>();
        Stagemanager.instance.canMove = true;
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
        if (Stagemanager.instance.canMove)
        {
            // 이부분은?
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotate);
        }  
        else
            Cookie.anim.SetBool("IsMove", false);
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
        if(Stagemanager.instance.canMove)
        {
            LootAtMouse();
            StartCoroutine(Cookie.Attack());
        }
    }

    void OnSkill()
    {
        if (Stagemanager.instance.canMove)
        {
            LootAtMouse();
            StartCoroutine(Cookie.Skill());
        }
    }
    IEnumerator OnUltimate()
    {
        if (Stagemanager.instance.canMove && Cookie.UltimateCT <= 0)
        {
            LootAtMouse();
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

    void LootAtMouse() //PC플랫폼일때만
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(mouseScreen);
        if(Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 dir = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            dir = dir - transform.position;

            Quaternion lookTarget = Quaternion.LookRotation(dir);
            rotate = lookTarget.eulerAngles;
            cc.transform.rotation = Quaternion.Euler(rotate);
        }
    }
    
}
