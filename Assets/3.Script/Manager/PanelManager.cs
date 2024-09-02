using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance { get; private set; }


    public GameObject Main;
    public GameObject Play;
    public GameObject Setting;
    public GameObject Dialog;
    public GameObject SceneChange;

    private Dictionary<string, GameObject> panelTable;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            panelTable = new Dictionary<string, GameObject>
            {
                { "Main", Main },
                { "Play", Play }
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

    public void PanelOpen(string panelName)
    {
        foreach(var row in panelTable)
        {
            row.Value.SetActive(row.Key.Equals(panelName));
        }
    }
}
