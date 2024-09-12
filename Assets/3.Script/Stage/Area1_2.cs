using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Area1_2 : MonoBehaviour
{
    public GameObject enemy;
    public CinemachineVirtualCamera CScam;
    public Animator Gate;

    int Fire = 2; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMove>(out PlayerMove a)) return;

        transform.GetComponent<BoxCollider>().enabled = false;
        enemy.SetActive(true);
        StartCoroutine(cutscene());
    }

    public void FireOn()
    {
        Fire -= 1;
        if(Fire == 0)
        {
            Gate.SetTrigger("Open");
        }
    }

    IEnumerator cutscene()
    {
        Stagemanager.instance.canMove = false;
        CScam.Priority = 11;
        yield return new WaitForSeconds(5);
        Stagemanager.instance.canMove = true;
        CScam.Priority = 9;
    }
}
