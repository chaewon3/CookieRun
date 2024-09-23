using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTag : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI ATK;

    private void Start()
    {
        name.text = FirebaseManager.instance.userData.username;
        // todo : ATK는 쿠키 정보 저장 후에 추가할 것.
    }
}
