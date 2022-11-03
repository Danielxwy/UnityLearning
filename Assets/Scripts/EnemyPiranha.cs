using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiranha : Enemy
{


    
    protected void Start()
    {
        leftx = left.position.x;
        rightx = right.position.x;
    }

    new protected void Update()
    {
        Anim();
        playerIn = Physics2D.OverlapCircle(transform.position, reactRadius, playerMask);
        playerx = player.position.x;
        if (attackTimer <= 0)     
        {
            Attack();
            isAttacking = false;
            attackTimer = attackCD;        //重新赋值
        }
        base.Update();  //运行父类Update
    }

    void Anim()
    {
        if(playerIn)
        {
            if (playerx > transform.position.x && !isAttacking)//Player在右且未攻击
            {TurnRight();}
            else if (playerx < transform.position.x && !isAttacking)//Player在左且未攻击
            { TurnLeft();}
            if(!isAttacking)
            {
                isAttacking = true;
                anim.SetBool("attacking", false);
            }
            else if(isAttacking)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else if(!playerIn)
        {
            anim.SetBool("attacking", false);
            attackTimer = attackCD;             //持续赋值
            isAttacking = false;
        }
    }


}
