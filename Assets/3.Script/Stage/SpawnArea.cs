using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public List<GameObject> colliders;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMove>(out PlayerMove a)) return;

        print("µé¾î¿È");
        foreach(GameObject col in colliders)
        {
            col.SetActive(true);
        }
    }
}
