using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    ICookie Cookie;

    float moveSpeed;
    float DashCooltime = 0;
    CharacterController cc;

    Vector3 dir = Vector3.zero;
    Vector3 rotate = Vector3.zero;

    private void Awake()
    {
        Cookie = GetComponentInChildren<ICookie>();
        cc = GetComponent<CharacterController>();
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

        dir.y += Physics.gravity.y * Time.deltaTime;
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
        StartCoroutine(Cookie.Attack());
    }

    void OnDash()
    {
        if (Cookie.DashCooltime <= 0)
        {
            transform.rotation = Quaternion.Euler(rotate);

            Quaternion rotation = Quaternion.Euler(rotate);
            Vector3 moveDir = rotation * Vector3.forward;

            StartCoroutine(Cookie.Dash(moveDir));            
        }
    }
    
}
