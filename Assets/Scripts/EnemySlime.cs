using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{

    new protected void Start()
    {

        base.Start();  //运行父类Start
    }

    new protected void Update()
    {
        playerIn = Physics2D.OverlapCircle(transform.position, reactRadius, playerMask);
        leftx = left.position.x;
        rightx = right.position.x;
        playerx = player.position.x;
        Anim();
        base.Update();  //运行父类Update
    }

    void Anim()
    {
        if(playerx > transform.position.x)
        {
            TurnRight();
        }
        else if(playerx < transform.position.x)
        {
            TurnLeft();
        }
    }


}
