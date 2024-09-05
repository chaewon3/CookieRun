using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "CookieRun/Add Enemy")]
public class EnemySO : ScriptableObject
{
    public int tableId;
    public GameObject ModelPrefab;

    public int HP;
    public int ATK;
    public int DEF;
    public float moveSpeed;

}
