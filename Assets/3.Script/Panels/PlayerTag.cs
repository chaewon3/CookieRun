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
        // todo : ATK�� ��Ű ���� ���� �Ŀ� �߰��� ��.
    }
}
