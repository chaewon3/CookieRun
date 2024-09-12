using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public GameObject CapturePL;
    Animator cookieanim;

    private void Awake()
    {
        cookieanim = CapturePL.GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        if(Gamemanager.instance.gameComplete)
           cookieanim.SetTrigger("Win");
        else
            cookieanim.SetTrigger("Loose");
    }
}
