using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public List<GameObject> walls = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMove>(out PlayerMove a)) return;

        if (walls.Count != 0)
        {
            foreach (GameObject col in walls)
            {
                col.SetActive(true);
            }
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.parent.gameObject.SetActive(true);
        }

        _ = GetComponent<BoxCollider>().enabled =false;
    }

    public void enemyDie(GameObject enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            if (walls.Count == 0) return;

            foreach (GameObject col in walls)
            {
                col.SetActive(false);
            }
        }
    }
}
