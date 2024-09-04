using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; set; }

    public bool canMove = false;
    // 로비 => 게임씬 들어갈때 쿠키어떤거 들고가는지 관리도 여기서.

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // todo : 테스트를 위해 ondestoryonload안넣었음 나중에 추가.
        }
    }
}
