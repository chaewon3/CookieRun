using System.Collections;
using UnityEngine;

public interface ICookie
{
    float moveSpeed { get; }
    float DashCooltime { get; }
    Animator anim { get; }

    IEnumerator Attack();

    void Skill();

    void Ultimate();

    IEnumerator Dash(Vector3 moveDir);

    
}
