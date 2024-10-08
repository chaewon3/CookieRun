using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCookie : MonoBehaviour, ICookie
{
    public float moveSpeed { get; set; } = 5;

    public float DashCT { get; set; } = 0;

    public float DashCoolTimeFillAmount { get; set; } = 1;

    public float SkillCT { get; set; } = 0;

    public float SkillCoolTimeFillAmount { get; set; } = 1;

    public float UltimateCT { get; set; } = 0;

    public float UltimateCoolTimeFillAmount { get; set; } = 1;

    public int CurrentHP { get; set; } = 0;

    public float HPPer { get; set; } = 0;

    public Sprite CutSceneIcon { get; set; }

    public Animator anim { get; set; }

    private IEnumerator Start()
    {
        anim = GetComponent<Animator>();

        yield return new WaitForSeconds(1.667f);
        Gamemanager.instance.canMove = true;
    }
    public IEnumerator Attack()
    {
        yield return null;
    }

    public IEnumerator Crashed(Vector3 direction)
    {
        yield return null;
    }

    public IEnumerator Dash(Vector3 moveDir)
    {
        yield return null;
    }

    public void Hit(int damage)
    {
        return;
    }

    public IEnumerator Skill()
    {
        yield return null;
    }

    public IEnumerator Ultimate()
    {
        yield return null;
    }
}
