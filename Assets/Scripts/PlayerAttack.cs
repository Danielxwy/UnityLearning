using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Unit player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            print(player.damage);
            if (transform.parent.localScale.x > 0)
            {
                collision.GetComponent<Enemy>().GetDamage(player.damage, Vector2.right);
            }else if(transform.parent.localScale.x < 0)
            {
                collision.GetComponent<Enemy>().GetDamage(player.damage, Vector2.left);
            }  
        }
    }
}
