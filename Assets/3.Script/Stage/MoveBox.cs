using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    GameObject MovingBox;
    bool canmove;

    private void Update()
    {
        if(canmove)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        canmove = true;
    }
}
