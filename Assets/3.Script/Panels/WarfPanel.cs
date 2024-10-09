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
            text.text = $"{(int)time + 1}�� ���� �ٸ� �÷��̾ ��ٸ��� ���Դϴ�.";
            yield return null;
        }
        text.text = $"��� �� <color=#FFC739>���� ����</color>���� �̵��մϴ�.";
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
