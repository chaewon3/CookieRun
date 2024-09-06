using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    List<GameObject> childs;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            childs.Add(child.gameObject);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMove>(out PlayerMove a)) return;

        foreach(GameObject col in childs)
        {
            col.SetActive(true);
        }

        _ = GetComponent<BoxCollider>().enabled =false;
    }

}
