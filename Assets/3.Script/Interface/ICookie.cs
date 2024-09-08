using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface ICookie
{
    float moveSpeed { get; }
    float DashCT { get; }
    float DashCoolTimeFillAmount { get; }
    float SkillCT { get; }
    float SkillCoolTimeFillAmount { get; }
    float UltimateCT { get; }
    float UltimateCoolTimeFillAmount { get; }
    int CurrentHP { get;}
    float HPPer { get; }
    Sprite CutSceneIcon { get; }
    Animator anim { get; }

    IEnumerator Attack();

    IEnumerator Skill();

    IEnumerator Ultimate();

    IEnumerator Dash(Vector3 moveDir);

    void Hit(int damage);
    ///die¤¤
}
