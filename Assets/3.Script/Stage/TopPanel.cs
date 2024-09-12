using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopPanel : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinText;

    public GameObject[] Jellies;
    public GameObject jellyprefab;
    public Sprite jellyOn;

    Dictionary<GameObject, Image> Jelly = new Dictionary<GameObject, Image>();

    private void Update()
    {
        int time = (int)Stagemanager.instance.time;
        timeText.text = $"{time / 60}:{time % 60}";
        coinText.text = Stagemanager.instance.Coins.ToString();
    }

    private void Awake()
    {
        for (int i = 0; i < Jellies.Length; i++)
        {
            GameObject image = Instantiate(jellyprefab, transform.GetChild(3));
            Jelly.Add(Jellies[i], image.GetComponent<Image>());
        }
    }

    public void GetJelly(GameObject jelly)
    {
        Jelly[jelly].sprite = jellyOn;
    }
}
