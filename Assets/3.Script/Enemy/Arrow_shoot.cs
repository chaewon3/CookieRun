using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_shoot : MonoBehaviour
{
    IEnemy enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<IEnemy>();
        transform.parent = null;
    }
    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }

    private void Update()
    {
        transform.position += transform.forward * 5 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerMove>(out PlayerMove cookie))
        {
            cookie.Cookie.Hit((int)enemy.ATK);
            Destroy(this.gameObject);
        }
        //if(TryGetComponent<Torch>(out Torch torch))

    }
}
