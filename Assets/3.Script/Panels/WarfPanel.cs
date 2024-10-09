using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class WarfPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject fadeout;
    public GameObject textImg;
    public PlayableDirector cutscene;
    public AudioClip bossenter;
    float time = 5;

    private void OnEnable()
    {
        StartCoroutine(warfMessage());
    }

    IEnumerator warfMessage()
    {
        textImg.SetActive(true);
        time = 5;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            text.text = $"{(int)time + 1}초 동안 다른 플레이어를 기다리는 중입니다.";
            yield return null;
        }
        text.text = $"잠시 후 <color=#FFC739>보스 구역</color>으로 이동합니다.";
        Gamemanager.instance.OnGame = false;
        yield return new WaitForSeconds(1f);
        fadeout.SetActive(true);
        yield return new WaitForSeconds(1f);
        textImg.SetActive(false);
        BGMManager.instance.BGMchange(bossenter);
        cutscene.Play();
        FindObjectOfType<Enemy_Boss_Gorilla>().CutScene();
        yield return new WaitForSeconds(4.3f);
        fadeout.SetActive(false); ;
        this.gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        fadeout.SetActive(false); ;
    }
}
