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
        float distance = Vector3.Distance(cookie.transform.position + Vector3.up * 0.5f, transform.position);
        if (distance <= 0.8f)
            Destroy(this.transform.parent.gameObject);

        yield return new WaitForSeconds(0.9f);
        bool get = false;
        float speed = 3;
        while(true)
        {
            distance = Vector3.Distance(cookie.transform.position, transform.position);
            Vector3 dir = cookie.transform.position+Vector3.up*0.5f - transform.position;
            transform.parent.position += dir.normalized * speed * Time.deltaTime;

            speed += 0.4f;

            if (distance <= 3f && !get)
            {
                get = true;
                anim.SetTrigger("Get");
                Destroy(this.transform.parent.gameObject,0.7f);
            }

            yield return null;
        }
    }
}
