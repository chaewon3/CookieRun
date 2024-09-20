using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleOnOFff : MonoBehaviour
{
    public Color onColor;
    public Color offColor;
    public TextMeshProUGUI text;
    public Image btnimg;
    public Image iconimg;

    public Material onmaterial;
    public Material offmaterial;

    private void Start()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(onoff);
    }
    void onoff(bool onoff)
    {
        btnimg.enabled = onoff;
        if (onoff)
        {
            text.fontMaterial = onmaterial;
            text.color = Color.white;
            iconimg.color = onColor;
        }
        else
        {
            text.fontMaterial = offmaterial;
            text.color = offColor;
            iconimg.color = offColor;
        }
    }

}
