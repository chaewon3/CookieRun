using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

using RandomNum = UnityEngine.Random;
public class Enemy_Boss_Gorilla : MonoBehaviourPunCallbacks, IEnemy
{
    public EnemySO data;
    public int ATK { get; set; }
    int HP;
    int DEF;
    public float HPPer => (float)HP / data.HP;

    Animator anim;
    bool canMove = false;
    float atkCT;
    int statelength;
    public Transform[] Players = new Transform[4];
    public Transform TargetPlayer;
    State currentState;

    float distance;
    Rigidbody rig;

    enum State
    {
        SkillRush,
        SkillThreat,
        SkillEat,
        SkillSmash,
        SkillDrop
    }

    private void Start()
    {
        statelength = Enum.GetValues(typeof(State)).Length;
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        StartCoroutine(Targeting());
    }

    private void Update()
    {
        // 마스터 클라이언트에서만 움직이고 동기화 할 예정
        if (!PhotonNetwork.IsMasterClient) return;

        if (TargetPlayer != null)
            distance = Vector3.Distance(rig.transform.position, TargetPlayer.position);
        if(canMove)
        {
            StartCoroutine(Attack());
            canMove = false; 
        }
    }

    public IEnumerator Attack()
    {
        //System.Random random = new System.Random();
        //int state = random.Next(statelength);
        //currentState = (State)state;

        //StartCoroutine($"{currentState.ToString()}");

        StartCoroutine(SkillRush());
        yield return new WaitForSeconds(8f);
        canMove = true;
    }
    public IEnumerator Targeting()
    {// 조건 살아있는동안으로 나중에바꾸기
        while(true)
        {
            int randomTarget = RandomNum.Range(0, 4);
            TargetPlayer = Players[randomTarget];
            yield return new WaitForSeconds(RandomNum.Range(7,15));
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 boxCenter = transform.position + transform.up + transform.forward*2;

        // BoxCast의 크기와 회전
        Vector3 boxSize = new Vector3(2.5f, 1f, 0.5f);
        Quaternion boxRotation = transform.rotation;

        // 기즈모 색상 설정
        Gizmos.color = Color.red;

        // 기즈모로 박스 그리기
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, boxSize);
    }
    public IEnumerator SkillRush()
    {
        Quaternion targetrotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
        transform.rotation = Quaternion.Euler(0, targetrotation.eulerAngles.y, 0);

        anim.SetTrigger(currentState.ToString());
        yield return new WaitForSeconds(1.5f);

        Vector3 boxCenter = transform.position + transform.up*3 + transform.forward;
        RaycastHit[] wallhits = Physics.BoxCastAll(boxCenter, new Vector3(1f, 1f, 0.5f),
            transform.forward, transform.rotation, 0.5f, 1 << 8);


        while (wallhits.Length == 0)
        {
            Move();
            yield return null;

            boxCenter = transform.position + transform.up * 3 + transform.forward;
            wallhits = Physics.BoxCastAll(boxCenter, new Vector3(1f, 1f, 0.5f),
                transform.forward, transform.rotation, 0.5f, 1 << 8);

            RaycastHit[] Plhits = Physics.BoxCastAll(boxCenter, new Vector3(2.5f, 1f, 0.5f),
                transform.forward, transform.rotation, 0.5f, 1 << 3);

            foreach(RaycastHit hit in Plhits)
            {
                PlayerRPC cc = hit.collider.GetComponent<PlayerRPC>();
                print(hit.collider.name);
                Vector3 pushDirection = (hit.collider.transform.position - transform.position).normalized;
                float pushForce = 20f; // 튕겨낼 힘의 크기
                Vector3 moveDirection = pushDirection * pushForce;

                // CharacterController를 사용하여 이동
                cc.PlayerMove(moveDirection);
            }

        }
        anim.SetTrigger("Bump");

    }
    public IEnumerator SkillThreat()
    {
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }
    public IEnumerator SkillEat()
    {
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }
    public IEnumerator SkillSmash()
    {
        if (distance > 3)
        {
            StartCoroutine(Attack());
            yield break;
        }
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }
    public IEnumerator SkillDrop()
    {
        anim.SetTrigger(currentState.ToString());
        yield return null;
    }

    public void Damaged(int damage)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Die()
    {
        throw new System.NotImplementedException();
    }

    public void Hit()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        rig.transform.position += rig.transform.forward * data.moveSpeed * Time.deltaTime;
    }
}
