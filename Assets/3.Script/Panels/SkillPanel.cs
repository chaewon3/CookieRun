using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillPanel : MonoBehaviour
{
    public Button AttackBtn;
    

    public Button DashBtn;
    public Image DashCT;
    public TextMeshProUGUI DashText;

    public Button SkillBtn;
    public Image SkillCT;
    public TextMeshProUGUI SkillText;

    public Button UltimateBtn;
    public Image UltimateCT;
    public TextMeshProUGUI UltimateText;

    public GameObject UltimateCutscne;

    public PlayerMove player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        player.UltimateAction += () =>
        {
            UltimateCutscne.SetActive(true);
            Invoke("Disable", 1);
        };
    }

    private void Update()
    {
        //´ë½Ã
        if (player.Cookie.DashCT > 0)
        {
            DashBtn.interactable = false;
            DashText.text = ((int)player.Cookie.DashCT+1).ToString();
            DashCT.fillAmount = player.Cookie.DashCoolTimeFillAmount;
        }
        else
        {
            DashBtn.interactable = true;
            DashText.text = string.Empty;
            DashCT.fillAmount = 0;
        }
        //½ºÅ³
        if (player.Cookie.SkillCT > 0)
        {
            SkillBtn.interactable = false;
            SkillText.text = ((int)player.Cookie.SkillCT + 1).ToString();
            SkillCT.fillAmount = player.Cookie.SkillCoolTimeFillAmount;
        }
        else
        {
            SkillBtn.interactable = true;
            SkillText.text = string.Empty;
            SkillCT.fillAmount = 0;
        }
        //±Ã
        if (player.Cookie.UltimateCT > 0)
        {
            UltimateBtn.interactable = false;
            UltimateText.text = ((int)player.Cookie.UltimateCT + 1).ToString();
            UltimateCT.fillAmount = player.Cookie.UltimateCoolTimeFillAmount;
        }
        else
        {
            UltimateBtn.interactable = true;
            UltimateText.text = string.Empty;
            UltimateCT.fillAmount = 0;
        }

        
    }

    void Disable()
    {
        UltimateCutscne.SetActive(false);
    }
}
