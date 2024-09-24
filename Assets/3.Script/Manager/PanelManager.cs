using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance { get; private set; }

    public CinemachineVirtualCamera raidcam;

    public MainPanel Main;
    public PlayPanel Play;
    public GameObject Story;
    public RaidPanel Raid;

    public GameObject Stageinfo;
    public GameObject invite;
    public GameObject Setting;
    public DialogPanel Dialog;
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
                { "Main", Main.gameObject },
                { "Play", Play.gameObject },
                { "Story", Story },
                { "StageInfo", Stageinfo },
                { "Raid", Raid.gameObject},
                { "Invite", invite},
                { "Setting", Setting },
                { "Dialog", Dialog.gameObject },
                {"SceneChange", SceneChange },
                {"Loading", Loading }
            };
        }
        SceneChange.SetActive(true);
        PanelChange("Main");
        PanelOpen("SceneChange");
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

    public void CreateParty()
    {
        Raid.CreatePartyButtonClick();
    }

    public void Invitation(string dialog, string roomname)
    {
        PanelOpen("Dialog");
        Dialog.onDialog(dialog, () =>
        {
            StartCoroutine(enterRoom(roomname));
        });
    }

    IEnumerator enterRoom(string roomname)
    {
        raidcam.Priority = 11;
        yield return new WaitForSeconds(0.2f);
        PanelChange("Raid");
        Raid.AttendanceBtnClick(roomname);
    }
}
