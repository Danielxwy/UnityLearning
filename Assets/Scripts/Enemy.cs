using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Unit
{

    public float attackCD;  
    public float health;

    private Vector2 dir;
    private AnimatorStateInfo info;

    protected bool playerIn;

    protected float attackTimer;

    [Header("受击速度补偿")]
    public float speed;  
    [Header("检测范围半径")]
    public float reactRadius;
    [Header("玩家图层")]
    public LayerMask playerMask;
    [Header("左右边界点及玩家位置")]
    public Transform left, right, player;
    [Header("左右边界点及玩家x轴值")]
    protected float playerx, leftx, rightx;

    protected void Update()
    {
        
        if (health <= 0)
        {
            anim.SetTrigger("dead");
        }
        info = anim.GetCurrentAnimatorStateInfo(0);

        if(isHit)
        {
            rb.velocity = dir * speed;
            if(info.normalizedTime >= 0.6f)
            {
                isHit = false;
            }
        }
    }

    public void GetDamage(float damage,Vector2 direction)     
    {
        health -= damage;
        isHit = true;
        this.dir = direction;
        Flash(flashTime);
        Instantiate(bloodFX, transform.position, Quaternion.identity);//BloodFX
        anim.SetTrigger("hurting");
        Tools.Instance.CameraShake();//屏幕震动               **************改重攻击震动
        StartCoroutine(FrameCut(0.1f));//受击反馈（抽帧
    }

    protected void TurnLeft()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    protected void TurnRight()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    protected void Attack()
    {
        anim.SetBool("attacking", true);
    }
}
