using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Gamemanager.instance.canMove = false;
        print("겜끝");
        //캐릭터 애니메이션 후 Panel열기
    }
}
