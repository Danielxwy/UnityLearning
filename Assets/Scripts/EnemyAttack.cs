using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private int enemyDamage;

    void Start()
    {
        enemyDamage = GetComponentInParent<Enemy>().damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (transform.parent.localScale.x > 0)
            {
                collision.GetComponent<PlayerController>().GetDamage(enemyDamage, Vector2.right);
            }else if (transform.parent.localScale.x < 0)
            {
                collision.GetComponent<PlayerController>().GetDamage(enemyDamage, Vector2.left);
            }
        }
    }
}
