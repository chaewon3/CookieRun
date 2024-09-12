using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Mission
{
    Clear,
    NoHit,
    ClearTime,
    AllTorch
}

public class Stagemanager : MonoBehaviour
{
    public static Stagemanager instance { get; set; }

    public Mission[] missions = new Mission[3];

    public string[] missionsText = new string[3];

    public int Coins { get; set; }

    public float maxTime;
    public float cleartime;
    public float time { get; private set; }
    public bool ClearGame { get; set; }

    public bool canMove { get; set; }
    public bool onGame { get; set; } = true;
    public Dictionary<GameObject, bool> Torch = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> Jelly = new Dictionary<GameObject, bool>();

    // �κ� => ���Ӿ� ���� ��Ű��� ������� ������ ���⼭.

    private void Start()
    {
        //todo : ���߿� ���⼭���� ������������ �ؾ���
        time = maxTime;
    }
    private void Update()
    {
        if(onGame)
         time -= Time.deltaTime;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    public bool ExecuteMission(int index)
    {
        switch(missions[index])
        {
            case Mission.Clear: return ClearGame;
            case Mission.NoHit: return false; 
            case Mission.ClearTime: return CheckClearTime(); 
            case Mission.AllTorch: return CheckTorch();
            default: return false;
        }
    }
    public void setJelly(GameObject jelly)
    {
        if (Jelly.ContainsKey(jelly))
        {
            Jelly[jelly] = true;
        }
        else
        {
            Jelly.Add(jelly, false);
        }
    }

    public void SetTorch(GameObject torch)
    {
        if (Torch.ContainsKey(torch))
        {
            Torch[torch] = true;
        }
        else
        {
            Torch.Add(torch, false);
        }
    }

    public bool CheckClearTime()
    {
        if (maxTime - time < cleartime && ClearGame)
            return true;

        return false;
    }

    public bool CheckTorch()
    {
        return Torch.Values.All(value => value);
    }

    
}
