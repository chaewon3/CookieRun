using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    bool canmove;
    public List<GameObject> paths;
    int index = 0;
    float movetime = 0.4f;

    float distance;

    CharacterController cc;
    private void Update()
    {
        if(canmove)
        {
            movetime -= Time.deltaTime;

            Vector3 Path = paths[index].transform.position;
            distance = Vector3.Distance(transform.position, Path);

            Vector3 dir = Path - transform.position;
            if(movetime <= 0)
            {
                transform.position += dir.normalized * 3 * Time.deltaTime;
                if (cc != null)
                    cc.Move(dir.normalized * 3 * Time.deltaTime);
                if (distance <= 0.1)
                {
                    index++;
                    if (index == paths.Count) index = 0;
                    movetime = 1.5f;
                }
            }        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        canmove = true;

        cc = other.GetComponent<CharacterController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        cc = null;
    }
}
