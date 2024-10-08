using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPPanel : MonoBehaviour
{
    public Slider HPBar;
    public GameObject twinkle;

    public void Refresh(float HPPer)
    {
        HPBar.value = HPPer;
        StopAllCoroutines();
        twinkle.SetActive(false);
        StartCoroutine(Twinkle());
    }

    IEnumerator Twinkle()
    {
        twinkle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        twinkle.SetActive(false);
    }
}
