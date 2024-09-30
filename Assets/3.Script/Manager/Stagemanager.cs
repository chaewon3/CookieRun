using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Stagemanager : MonoBehaviour
{
    public StageData data;
    public static Stagemanager instance { get; set; }

    Mission[] missions = new Mission[3];

    public string[] missionsText = new string[3];

    public int Coins { get; set; }

    public float maxTime;
    float cleartime;
    public float time { get; private set; }
    public bool ClearGame { get; set; }

    public bool onGame { get; set; } = true;
    public Dictionary<GameObject, bool> Torch = new Dictionary<GameObject, bool>();
    public Dictionary<GameObject, bool> Jelly = new Dictionary<GameObject, bool>();

    // 로비 => 게임씬 들어갈때 쿠키어떤거 들고가는지 관리도 여기서.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        data = Gamemanager.instance.CurrentStage;

        print(data.Data.Stagename);
        missions[0] = data.Data.Mission_1;
        missionsText[0] = data.Data.Mission_1Text;
        missions[1] = data.Data.Mission_2;
        missionsText[1] = data.Data.Mission_2Text;
        missions[2] = data.Data.Mission_3;
        missionsText[2] = data.Data.Mission_3Text;

        maxTime = data.Data.time;
        cleartime = data.Data.clearTime;
            
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        time = maxTime;

        int i = 0;
        List<GameObject> keys = new List<GameObject>(Jelly.Keys);
        
        foreach (var key in keys)
        {
            Jelly[key] = data.Jellies[i];
            i++;
        }
    }
    private void Update()
    {
        if(onGame)
         time -= Time.deltaTime;
    }
    
    
    public bool ExecuteMission(int index)
    {
        if (data.Missions[index]) return true;

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
