using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Area1_2 area;
    public GameObject fireEffect;

    bool onFire = false;
    public void Fire()
    {
        if (onFire) return;

        onFire = true;
        fireEffect.SetActive(true);
        area.FireOn();
    }
}
