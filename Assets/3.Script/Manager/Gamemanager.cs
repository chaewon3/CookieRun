using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; set; }

    public bool canMove = false;
    // �κ� => ���Ӿ� ���� ��Ű��� ������� ������ ���⼭.

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // todo : �׽�Ʈ�� ���� ondestoryonload�ȳ־��� ���߿� �߰�.
        }
    }
}
