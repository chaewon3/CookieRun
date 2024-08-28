using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    float moveSpeed = 3f;
    CharacterController cc;
    Animator anim;

    Vector3 dir = Vector3.zero;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (cc.isGrounded)
        //{
        //    var h = Input.GetAxisRaw("Horizontal");
        //    var v = Input.GetAxisRaw("Vertical");

        //    dir = new Vector3(h, 0, v) * moveSpeed * -1;

        //    if (dir != Vector3.zero)
        //    {
        //        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        //        //애니메이션
        //    }
        //    else
        //    { }
        //}

        dir.y += Physics.gravity.y * Time.deltaTime;
        cc.Move(dir * moveSpeed * Time.deltaTime);



    }


    void OnMove(InputValue value)
    {
        Vector3 input = -value.Get<Vector2>();
        if(cc.isGrounded)
        {
            dir = new Vector3(input.x, 0, input.y);

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, 0);
                anim.SetBool("IsMove", true);
            }
            else
                anim.SetBool("IsMove", false);

    }
    }
}
