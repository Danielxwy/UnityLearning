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

    [Header("�ܻ��ٶȲ���")]
    public float speed;  
    [Header("��ⷶΧ�뾶")]
    public float reactRadius;
    [Header("���ͼ��")]
    public LayerMask playerMask;
    [Header("���ұ߽�㼰���λ��")]
    public Transform left, right, player;
    [Header("���ұ߽�㼰���x��ֵ")]
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
        Tools.Instance.CameraShake();//��Ļ��               **************���ع�����
        StartCoroutine(FrameCut(0.1f));//�ܻ���������֡
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
