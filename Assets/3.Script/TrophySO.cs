using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Trophy", menuName = "CookieRun/Add Trophy")]
public class TrophySO : ScriptableObject
{
    public Sprite icon;
    public string Text;
}
