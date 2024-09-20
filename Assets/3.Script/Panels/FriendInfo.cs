using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FriendInfo : MonoBehaviour
{
    [HideInInspector]
    public UserData data;

    public TextMeshProUGUI Level;
    public TextMeshProUGUI name;

    private void Start()
    {
        Level.text = data.level.ToString();
        name.text = data.username;
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}