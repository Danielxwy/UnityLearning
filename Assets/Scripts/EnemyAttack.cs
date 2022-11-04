using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Unit enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (transform.parent.localScale.x > 0)
            {
                collision.GetComponent<PlayerController>().GetDamage(enemy.damage, Vector2.right);
            }else if (transform.parent.localScale.x < 0)
            {
                collision.GetComponent<PlayerController>().GetDamage(enemy.damage, Vector2.left);
            }
        }
    }
}
