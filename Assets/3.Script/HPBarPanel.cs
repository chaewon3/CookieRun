using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarPanel : MonoBehaviour
{
    public static HPBarPanel instance;

    [SerializeField] GameObject HPbarPrefab = null;

    public List<Transform> EnemyList = new List<Transform>();
    List<GameObject> HPbarList = new List<GameObject>();

    public Camera MainCam;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // todo: set연결하면 지워야함
        for (int i = 0; i < EnemyList.Count; i++)
        {
            GameObject HPbar = Instantiate(HPbarPrefab, EnemyList[i].transform.position, Quaternion.identity, transform);
            HPbarList.Add(HPbar);
        }
    }

    private void Update()
    {
        // hp 연동 로직 ?
        for(int i = 0; i < EnemyList.Count; i++)
        {
            Slider HPbar = HPbarList[i].GetComponent<Slider>();
            IEnemy enemy = EnemyList[i].GetComponent<IEnemy>();
            HPbar.value = enemy.HPPer;
        }
    }

    public void SetHPPanel(Transform enemies)
    {
        EnemyList.Add(enemies);
        GameObject HPbar = Instantiate(HPbarPrefab, enemies.position, Quaternion.identity, transform);
        HPbarList.Add(HPbar);

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
