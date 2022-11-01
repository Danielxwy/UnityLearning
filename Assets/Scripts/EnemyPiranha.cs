using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiranha : Enemy
{


    
    new protected void Start()
    {
        leftx = left.position.x;
        rightx = right.position.x;
        base.Start();  //运行父类Start
    }

    new protected void Update()
    {
        Anim();
        playerIn = Physics2D.OverlapCircle(transform.position, reactRadius, playerMask);
        playerx = player.position.x;
        if (attackTimer <= 0)     
        {
            Attack();
            isAttack = false;
            attackTimer = attackCD;        //重新赋值
        }
        base.Update();  //运行父类Update
    }

    void Anim()
    {
        if(playerIn)
        {
            if (playerx > transform.position.x && !isAttack)//Player在右且未攻击
            {TurnRight();}
            else if (playerx < transform.position.x && !isAttack)//Player在左且未攻击
            { TurnLeft();}
            if(!isAttack)
            {
                isAttack = true;
                anim.SetBool("attacking", false);
            }
            else if(isAttack)
            {
                attackTimer -= Time.deltaTime;
            }
        }
        else if(!playerIn)
        {
            anim.SetBool("attacking", false);
            attackTimer = attackCD;             //持续赋值
            isAttack = false;
        }
    }


}
