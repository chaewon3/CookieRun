using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float moveSpeed = 5f;
    CharacterController cc;

    Vector3 dir = Vector3.zero;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //print(cc.isGrounded);
        //if (cc.isGrounded)
        //{
        //    dir.x = Input.GetAxisRaw("Horizontal");
        //    dir.z = Input.GetAxisRaw("Vertical");


        //    //var h = Input.GetAxis("Horizontal");
        //    //var v = Input.GetAxis("Vertical");

        //    print(dir);
        //    //dir = new Vector3(h, 0, v) * moveSpeed;
        //    if (dir != Vector3.zero)
        //    {
        //       // transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        //        //애니메이션
        //    }
        //    else
        //    { }
        //}

        //dir.y += Physics.gravity.y * Time.deltaTime;
        //cc.Move(dir *moveSpeed* Time.deltaTime);

        print($"{0 == Input.GetAxisRaw("Horizontal")}");

    }
}
