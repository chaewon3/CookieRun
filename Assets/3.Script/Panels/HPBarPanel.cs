using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class HPBarPanel : MonoBehaviour
{
    public static HPBarPanel instance;

    [SerializeField] GameObject HPbarPrefab = null;
    [SerializeField] GameObject DamageTextPrefab;
    [SerializeField] GameObject RPCHPbarPrefab;

    List<Transform> EnemyList = new List<Transform>();
    List<GameObject> HPbarList = new List<GameObject>();
    Dictionary<Transform, GameObject> HPBarList = new Dictionary<Transform, GameObject>();
    public Camera MainCam;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetEnemyHP(Transform enemy)
    {
        EnemyList.Add(enemy);
        GameObject HPbar = Instantiate(HPbarPrefab, enemy.position, Quaternion.identity, base.transform);
        HPbarList.Add(HPbar);
    }

    public void RefreshEnemyHP(Transform enemy, int damage)
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

    public void DamageText(Transform enemy, int damage)
    {
        GameObject DamageText = Instantiate(DamageTextPrefab, transform);
        DamageText.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        DamageText.transform.position = MainCam.WorldToScreenPoint(
            enemy.position + new Vector3(Random.Range(-1f, 1f), Random.Range(0.8f, 1.2f), 0));

        Destroy(DamageText, .5f);
    }

    public void RemoveEnemyHP(Transform enemies)
    {
        Destroy(HPBarList[enemies]);
        HPBarList.Remove(enemies);

        int index = EnemyList.IndexOf(enemies);
        if(index != -1)
        {
            Destroy(HPbarList[index]);
            HPbarList.RemoveAt(index);
            EnemyList.RemoveAt(index);
        }
    }

    public void SetRPCHP(Transform transform, Player player)
    {
        GameObject HPbar = Instantiate(RPCHPbarPrefab, transform.position, Quaternion.identity, base.transform);
        HPBarList.Add(transform, HPbar);

        if (HPbar.TryGetComponent<RPCHpBar>(out var HP))
        {
            HP.playerTransform = transform;
            HP.player = player;
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
