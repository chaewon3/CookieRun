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
    public int coin;

    public AudioClip ATK1;
    public AudioClip ATK2;
    public AudioClip ATK3;
    public AudioClip RushStart;
    public AudioClip RushLoop;
    public AudioClip RushEnd;
    public AudioClip Drop;
}
