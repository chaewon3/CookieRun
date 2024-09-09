using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_shoot : MonoBehaviour
{
    IEnemy enemy;

    private void Awake()
    {
        enemy = transform.parent.GetComponent<IEnemy>();        
    }
    private void Start()
    {
        Destroy(this.gameObject, 3f);
        transform.parent = null;
    }

    private void Update()
    {
        transform.position += transform.forward * 5 * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.TryGetComponent<PlayerMove>(out PlayerMove cookie))
        {
            cookie.Cookie.Hit((int)enemy.ATK);
            Destroy(this.gameObject);
        }

        if(other.TryGetComponent<Torch>(out Torch torch))
        {
            torch.Fire();
            Destroy(this.gameObject);
        }

    }
}
