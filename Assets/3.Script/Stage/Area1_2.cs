using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Area1_2 : MonoBehaviour
{
    public GameObject enemy;
    public CinemachineVirtualCamera CScam;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMove>(out PlayerMove a)) return;

        enemy.SetActive(true);
        StartCoroutine(cutscene());
    }

    IEnumerator cutscene()
    {
        Gamemanager.instance.canMove = false;
        CScam.Priority = 11;
        yield return new WaitForSeconds(5);
        Gamemanager.instance.canMove = true;
        CScam.Priority = 9;
    }
}
