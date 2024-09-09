using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyPanel : MonoBehaviour
{
    public GameObject[] Jellies;
    public GameObject jellyprefab;
    public Sprite jellyOn;

    Dictionary<GameObject, Image> Jelly = new Dictionary<GameObject, Image>();

    private void Awake()
    {
        for(int i=0;i<Jellies.Length;i++)
        {
            GameObject image = Instantiate(jellyprefab, transform);
            Jelly.Add(Jellies[i], image.GetComponent<Image>());
        }
    }

    public void GetJelly(GameObject jelly)
    {
        Jelly[jelly].sprite = jellyOn;
    }
}
