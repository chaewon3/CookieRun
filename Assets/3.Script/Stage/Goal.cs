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
        print("�׳�");
        //ĳ���� �ִϸ��̼� �� Panel����
    }
}
