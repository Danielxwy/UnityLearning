using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject bloodFX;
    public float damage;
    public float flashTime;//受击闪烁时间
    public float maxHealth;
    public float currentHealth;

    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer render;

    protected bool isGround, isDashing, isAttacking,
                 isHit, jumpPressed, isHanging;
    public void Flash(float time)//人物闪烁
    {
        render.color = Color.red;
        Invoke("FlashBack", time);
    }

    public void FlashBack()
    {
        render.color = Color.white;
    }

    public IEnumerator FrameCut(float pauseTime)//抽帧
    {
        anim.speed = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        anim.speed = 1;
    }


    #region 帧事件
    void Destroy()
    {
        Destroy(gameObject);
        Time.timeScale = 0.5f;
    }

    void IsHitFalse()
    {
        isHit = false;
    }
    void AttackOver()
    {
        isAttacking = false;
    }
    void Dead()
    {
        Destroy(gameObject);
    }
    #endregion
}
