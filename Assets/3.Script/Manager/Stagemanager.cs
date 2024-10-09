using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Stagemanager : MonoBehaviour
{
    public StageData stagedata;
    public CookieData cookiedata;
    public static Stagemanager instance { get; set; }

    public Transform playerposition;
    public Transform captureposition;
    public GameObject resultpanel;
    Mission[] missions = new Mission[3];
    [HideInInspector]
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
        time = maxTime;
        if (instance == null)
        {
            instance = this;
        }
        stagedata = Gamemanager.instance.CurrentStage;
        cookiedata = Gamemanager.instance.cookie;

        var player = Instantiate(cookiedata.Data.ModelPrefab, playerposition).GetComponent<CookieBase>();
        var pacture = Instantiate(cookiedata.Data.LobbyPrefab, captureposition);
        player.Cookie = cookiedata;
        print(stagedata.Data.Stagename);
        missions[0] = stagedata.Data.Mission_1;
        missionsText[0] = stagedata.Data.Mission_1Text;
        missions[1] = stagedata.Data.Mission_2;
        missionsText[1] = stagedata.Data.Mission_2Text;
        missions[2] = stagedata.Data.Mission_3;
        missionsText[2] = stagedata.Data.Mission_3Text;

        maxTime = stagedata.Data.time;
        cleartime = stagedata.Data.clearTime;
            
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        int i = 0;
        List<GameObject> keys = new List<GameObject>(Jelly.Keys);
        
        foreach (var key in keys)
        {
            Jelly[key] = stagedata.Jellies[i];
            i++;
        }
    }
    private void Update()
    {
        if(onGame)
         time -= Time.deltaTime;
        if(time <= 0 && onGame)
        {
            var cookie = FindObjectOfType<PlayerMove>().GetComponentInChildren<Animator>();
            Gamemanager.instance.canMove = false;
            cookie.SetTrigger("Die"); StartCoroutine(endGame());
        }
    }
    
    public IEnumerator endGame()
    {
        onGame = false;
        yield return new WaitForSeconds(1.2f);
        resultpanel.SetActive(true);
    }
    public bool ExecuteMission(int index)
    {
        if (stagedata.Missions[index]) return true;

        switch(missions[index])
        {
            case Mission.Clear: stagedata.Missions[index] = ClearGame; return ClearGame;
            case Mission.NoHit: return false; 
            case Mission.ClearTime: return CheckClearTime(index); 
            case Mission.AllTorch: return CheckTorch(index);
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

    public bool CheckClearTime(int index)
    {
        if (maxTime - time < cleartime && ClearGame)
        {
            stagedata.Missions[index] = true;
            return true;
        }            

        return false;
    }

    public bool CheckTorch(int index)
    {
        stagedata.Missions[index] = Torch.Values.All(value => value);
        return Torch.Values.All(value => value);
    }

    
}
