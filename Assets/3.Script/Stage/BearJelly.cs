using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearJelly : MonoBehaviour
{
    Animator anim;
    Collider col;
    JellyPanel panel;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        panel = FindObjectOfType<JellyPanel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        panel.GetJelly(this.gameObject);
        anim.SetTrigger("Get");
        col.enabled = false;    
    }
}
