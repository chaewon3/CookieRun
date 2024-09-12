using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Area1_2 area;
    public GameObject fireEffect;

    bool onFire = false;
    private void Start()
    {
        Stagemanager.instance.SetTorch(gameObject);
    }

    public void Fire()
    {
        if (onFire) return;

        onFire = true;
        fireEffect.SetActive(true);
        area.FireOn();
        Stagemanager.instance.SetTorch(gameObject);
    }
}
