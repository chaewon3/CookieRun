using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearJelly : MonoBehaviour
{
    Animator anim;
    Collider col;
    TopPanel panel;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        panel = FindObjectOfType<TopPanel>();
    }
    private void Start()
    {
        Stagemanager.instance.setJelly(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        panel.GetJelly(this.gameObject);
        Stagemanager.instance.setJelly(gameObject);
        anim.SetTrigger("Get");
        col.enabled = false;    
    }
}
