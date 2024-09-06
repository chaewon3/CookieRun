using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int HPPer { get; }
    void Move();
    IEnumerator Attack();
    void Hit(int damage, float nuckback);
    IEnumerator Die();
}
