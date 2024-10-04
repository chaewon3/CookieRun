using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarfPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject fadeout;
    public GameObject textImg;
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
        textImg.SetActive(false);
        yield return new WaitForSeconds(2.3f);
        fadeout.SetActive(false); ;
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        fadeout.SetActive(false); ;
    }
}
