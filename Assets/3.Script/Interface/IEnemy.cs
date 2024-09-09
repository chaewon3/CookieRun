using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int ATK { get; set; }
    float HPPer { get; }
    void Move();
    IEnumerator Attack();
    void Damaged(int damage);
    void Hit();
    IEnumerator Die();
}
