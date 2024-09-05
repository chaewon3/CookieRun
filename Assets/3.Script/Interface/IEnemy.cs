using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Move();
    void Attack();
    void Hit(int damage);
    void Die();
}
