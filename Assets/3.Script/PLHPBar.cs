using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLHPBar : MonoBehaviour
{
    public Transform Player;

    Camera MainCam;

    private void Start()
    {
        MainCam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = MainCam.WorldToScreenPoint(Player.position + new Vector3(0, 3f, 0));
    }
}
