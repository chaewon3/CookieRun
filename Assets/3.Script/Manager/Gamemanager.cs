using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stageManager�� �ٲ��� ���..
public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance { get; set; }

    public int Coins { get; set; }
    public float time { get; private set; }
    public bool canMove = false;
    // �κ� => ���Ӿ� ���� ��Ű��� ������� ������ ���⼭.

    private void Start()
    {
        time = 720;
    }
    private void Update()
    {
        time -= Time.deltaTime;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // todo : �׽�Ʈ�� ���� ondestoryonload�ȳ־��� ���߿� �߰�.
        }
    }
}
