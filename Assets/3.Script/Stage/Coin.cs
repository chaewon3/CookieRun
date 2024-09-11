using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coin = 100;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerMove>()) return;

        Gamemanager.instance.Coins += coin;
        StartCoroutine(getcoin(other));
    }

    IEnumerator getcoin(Collider cookie)
    {
        bool get = false;
        float speed = 3;
        while(true)
        {
            float distance = Vector3.Distance(cookie.transform.position, transform.position);

            Vector3 dir = cookie.transform.position+Vector3.up*0.5f - transform.position;
            transform.parent.position += dir.normalized * speed * Time.deltaTime;

            if (speed < 6)
                speed += 0.2f;

            if (distance <= 3f && !get)
            {
                get = true;
                anim.SetTrigger("Get");
                Destroy(this.gameObject,0.7f);
            }
            if (distance <= 0.5f)
                Destroy(this.gameObject);
            yield return null;
        }
    }
}
