using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public float attackCD;  
    public GameObject bloodFX;
    public int health;
    public int damage;

    new private SpriteRenderer renderer;
    private Vector2 dir;
    private bool isHit;
    private AnimatorStateInfo info;
    private Rigidbody2D rb;

    protected bool playerIn;
    protected float attackTimer;
    protected bool isAttack;

    [Header("�ܻ��ٶȲ���")]
    public float speed;  
    [Header("��ⷶΧ�뾶")]
    public float reactRadius;
    [Header("�ܻ���˸ʱ��")]
    public float flashTime;
    [Header("������")]
    public Animator anim;
    [Header("���ͼ��")]
    public LayerMask playerMask;
    [Header("���ұ߽�㼰���λ��")]
    public Transform left, right, player;
    [Header("���ұ߽�㼰���x��ֵ")]
    protected float playerx, leftx, rightx;

    

    protected void Start()
    {
        
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        
        if (health <= 0)
        {
            anim.SetTrigger("dying");
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

    public void GetDamage(int playerDamage,Vector2 direction)     
    {
        health -= playerDamage;
        isHit = true;
        this.dir = direction;
        Flash(flashTime);
        Instantiate(bloodFX, transform.position, Quaternion.identity);//BloodFX
        anim.SetTrigger("hurting");
        CameraShake.instance.Shake();//��Ļ��                                    **************���ع�����
        StartCoroutine(FrameCut(0.1f));//�ܻ���������֡
    }

    private IEnumerator FrameCut(float pauseTime)
    {
        anim.speed = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        anim.speed = 1;
    }    

    void Dead()//�����¼�
    {
        Destroy(gameObject);
    }

    protected void Flash(float time)
    {
        renderer.color = Color.red;
        Invoke("FlashBack", time);
    }

    protected void FlashBack()
    {
        renderer.color = Color.white;
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
