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


    private IEnumerator Start()
    {
        cc = transform.GetComponent<CharacterController>();
        while (Cookie == null)
        {
            Cookie = GetComponentInChildren<ICookie>();
            yield return null;
        }
        moveSpeed = Cookie.moveSpeed;
        Gamemanager.instance.canMove = true;
    }

    private void Update()
    {
        if (Cookie == null || !Gamemanager.instance.OnGame)
            return;
        print(transform.position);
        cc.Move(cc.transform.up * Physics.gravity.y * Time.deltaTime);
        if (Gamemanager.instance.canMove)
        {
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotate);
        }  
        else
            Cookie.anim.SetBool("IsMove", false);
    }


    public void OnMove(InputValue value)
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

    public void OnAttack()
    {
        if(Gamemanager.instance.canMove)
        {
            LootAtMouse();
            StartCoroutine(Cookie.Attack());
        }
    }

    public void OnSkill()
    {
        if (Gamemanager.instance.canMove)
        {
            LootAtMouse();
            StartCoroutine(Cookie.Skill());
        }
    }
    IEnumerator OnUltimate()
    {
        if (Gamemanager.instance.canMove && Cookie.UltimateCT <= 0)
        {
            LootAtMouse();
            StartCoroutine(Cookie.Ultimate());
            yield return new WaitForSeconds(0.1f);
            UltimateAction?.Invoke();
        }
    }
    public void OnDash()
    {
        if (Cookie.DashCT <= 0)
        {
            transform.rotation = Quaternion.Euler(rotate);

            Quaternion rotation = Quaternion.Euler(rotate);
            Vector3 moveDir = rotation * Vector3.forward;

            StartCoroutine(Cookie.Dash(moveDir));            
        }
    }

    public void LootAtMouse() //PCÇÃ·§ÆûÀÏ¶§¸¸
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

    public void Warf(Transform transform)
    {
        print("Á¦¹ß");
        cc.transform.position = transform.position;
        cc.transform.rotation = transform.rotation;
    }
   
}
