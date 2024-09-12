using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public GameObject CapturePL;

    public TextMeshProUGUI timeText;

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

        float time = Gamemanager.instance.time;
        timeText.text = $"{(int)time / 60}:{(time % 60).ToString("F2")}";
    }
}
