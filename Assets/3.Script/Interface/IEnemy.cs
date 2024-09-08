using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    float HPPer { get; }
    void Move();
    IEnumerator Attack();
    void Hit(int damage, float nuckback=0);
    IEnumerator Die();
}
