using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public int ATK;
    public float pushForce = 10f;
    Collider collider;
    public GameObject warning;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        var localPL = other.GetComponentInChildren<CookieBase>();
        if (localPL == null) return;
        collider.enabled = false;
        Vector3 pushDirection = (other.transform.position - transform.position).normalized;
        pushDirection.y = 0;
        Vector3 moveDirection = pushDirection * pushForce;

        StartCoroutine(localPL.Crashed(moveDirection));
        localPL.Hit(ATK);

    }

}
