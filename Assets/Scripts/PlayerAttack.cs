using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private int playerDamage;
    
    void Start()
    {
        playerDamage = GetComponentInParent<PlayerController>().damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if (transform.parent.localScale.x > 0)
            {
                collision.GetComponent<Enemy>().GetDamage(playerDamage,Vector2.right);
            }else if(transform.parent.localScale.x < 0)
            {
                collision.GetComponent<Enemy>().GetDamage(playerDamage, Vector2.left);
            }  
        }
    }
}
