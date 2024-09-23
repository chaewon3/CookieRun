using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance { get; private set; }


    public GameObject Main;
    public GameObject Play;
    public GameObject Stage;
    public GameObject Stageinfo;
    public GameObject invite;

    public GameObject Setting;
    public GameObject Dialog;
    public GameObject SceneChange;
    public GameObject Loading;

    private Dictionary<string, GameObject> panelTable;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            panelTable = new Dictionary<string, GameObject>
            {
                { "Main", Main },
                { "Play", Play },
                { "Stage", Stage },
                { "StageInfo", Stageinfo },
                { "Invite", invite},
                { "Setting", Setting },
                { "Dialog", Dialog },
                {"SceneChange", SceneChange },
                {"Loading", Loading }
            };
        }
        SceneChange.SetActive(true);
        PanelOpen("Main");
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.2f);
        SceneChange.SetActive(false);
        if (FirebaseManager.instance.userData.username == string.Empty)
        {
            Setting.SetActive(true);
        }
    }

    public void PanelChange(string panelName)
    {
        foreach(var row in panelTable)
        {
            row.Value.SetActive(row.Key.Equals(panelName));
        }
    }

    public void PanelOpen(string panelName)
    {
        panelTable[panelName].SetActive(true);
    }

    public IEnumerator ChangeAnimation(string panelName)
    {
        PanelOpen("SceneChange");
        SceneChange.GetComponent<Animator>().SetTrigger("close");
        yield return new WaitForSeconds(1);
        PanelOpen(panelName);
        yield return new WaitForSeconds(1);
        SceneChange.SetActive(false);
    }
}
