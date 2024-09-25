using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Tagtype
{
    nameTag,
    raidProfile
}

public class PlayerTag : MonoBehaviour
{
    public Tagtype Tag;
    public TextMeshProUGUI name;
    public TextMeshProUGUI ATK;
    public Color color;
    public Image BGimg;
    public Image portrait;
    public bool localplayer = false;

    private void Start()
    {
        if (localplayer)
        {
            if(Tag == Tagtype.nameTag)
               BGimg.color = color;
            if (Tag == Tagtype.raidProfile)
            {
                name.color = color;
            }
        }
    }
}
