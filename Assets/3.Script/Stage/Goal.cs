using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal : MonoBehaviour
{
    public GameObject Blur;

    public GameObject chosenEffect;
    public CanvasGroup canvas;
    public GameObject completePanel;
    public GameObject ResultPanel;

    float loopTimeLimit = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        Gamemanager.instance.canMove = false;
        Stagemanager.instance.onGame = false;
        Stagemanager.instance.ClearGame = true;
        StartCoroutine(Effect());
        //캐릭터 애니메이션 후 Panel열기
    }

    IEnumerator Effect()
    {
        yield return new WaitForSeconds(0.3f);
        canvas.interactable = false;
        float starttime = 0;

        while (starttime < 0.8f)
        {
            canvas.alpha = Mathf.Lerp(1, 0, starttime / 0.8f);
            starttime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0;
        canvas.gameObject.SetActive(false);

        GameObject effect = (GameObject)Instantiate(chosenEffect);
        effect.transform.position = transform.position+Vector3.up;

        yield return new WaitForSeconds(loopTimeLimit);
        completePanel.SetActive(true);
        Destroy(effect);
        yield return new WaitForSeconds(1.8f);
        completePanel.SetActive(false);

        yield return new WaitForSeconds(.2f);
        Blur.SetActive(true);
        ResultPanel.SetActive(true);
    }


}
