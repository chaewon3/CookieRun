using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarPanel : MonoBehaviour
{
    public static HPBarPanel instance;

    [SerializeField] GameObject HPbarPrefab = null;
    [SerializeField] GameObject DamageTextPrefab;

    List<Transform> EnemyList = new List<Transform>();
    List<GameObject> HPbarList = new List<GameObject>();

    public Camera MainCam;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetHPPanel(Transform enemies)
    {
        EnemyList.Add(enemies);
        GameObject HPbar = Instantiate(HPbarPrefab, enemies.position, Quaternion.identity, transform);
        HPbarList.Add(HPbar);        

    }

    public void RefreshHP(Transform enemy, int damage)
    {
        int index = EnemyList.IndexOf(enemy);

        Slider HPbar = HPbarList[index].GetComponent<Slider>();
        IEnemy ienemy = EnemyList[index].GetComponent<IEnemy>();
        HPbar.value = ienemy.HPPer;
        HPbar.transform.position =
            MainCam.WorldToScreenPoint(enemy.position + new Vector3(0, 1.5f, 0));


        GameObject DamageText = Instantiate(DamageTextPrefab, HPbar.gameObject.transform);
        DamageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        DamageText.transform.position = MainCam.WorldToScreenPoint(
            enemy.position + new Vector3(Random.Range(-0.3f,0.3f),Random.Range(0.8f, 1.2f), 0));

        Destroy(DamageText, .5f);        
    }

    public void RemoveHPPanel(Transform enemies)
    {
        int index = EnemyList.IndexOf(enemies);
        if(index != -1)
        {
            Destroy(HPbarList[index]);
            HPbarList.RemoveAt(index);
            EnemyList.RemoveAt(index);
        }
    }

    
    private void LateUpdate()
    {
        if (EnemyList is null || HPbarList is null) return;

        for(int i = 0; i < EnemyList.Count;i++)
        {
            HPbarList[i].transform.position =
                MainCam.WorldToScreenPoint(EnemyList[i].position + new Vector3(0, 1.5f, 0));
        }
    }
}