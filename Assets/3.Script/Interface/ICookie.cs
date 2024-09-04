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
    Sprite CutSceneIcon { get; }
    Animator anim { get; }

    IEnumerator Attack();

    IEnumerator Skill();

    IEnumerator Ultimate();

    IEnumerator Dash(Vector3 moveDir);

    
}
