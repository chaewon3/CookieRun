using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    bool canmove;
    public List<GameObject> paths;
    int index = 0;

    float distance;
    private void Update()
    {
        if(canmove)
        {
            Vector3 Path = paths[index].transform.position;
            distance = Vector3.Distance(transform.position, Path);
            
            //if(distance <= 0)


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        canmove = true;
    }
}
