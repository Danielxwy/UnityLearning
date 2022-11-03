using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Unit
{
    [Header("速度补偿")]
    public float attackSpeed;
    public float hurtSpeed;

    [Header("人物参数")]
    public float speed;
    public float jumpForce;
    public float hangingJumpForce;//悬挂时跳跃的力
    private int jumpCount;

    [Header("连击参数")]
    public float interval;//连击CD
    private int attackCombo;
    private float timer;

    [Header("冲刺参数")]
    public float dashTime;//冲刺持续时间
    private float dashTimeLeft;//冲刺剩余时间
    private float lastDashTime = -10f;//上一次冲刺时间（用来做CD）
    public float dashCD;
    public float dashSpeed;

    [Header("悬挂射线检测")]
    public float playerHeight = 0.75f;
    public float playerEyeHeight = 0.4f;
    public float grabDistance = 0.2f;//悬挂需离墙的距离
    public float topRaycast = 0.7f;//头部往前一点向下的射线，检测头顶无障碍

    //public Transform groundCheck;
    public LayerMask layer;
    public LayerMask oneWay;

    private BoxCollider2D coll;
    private bool canDown;
    private bool isOnOneWay;

    Vector2 dir;

    public float CurrentHealth { get => currentHealth; 
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                anim.SetTrigger("dead");
                GameController.isGameOver = true;
                rb.velocity = Vector2.zero;
            }
        }
    }

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        CurrentHealth = maxHealth;
    }

    void Update()
    {
        if (!GameController.isGameOver)
        {
            Anim();
            Hang();
            IsGroundCheck();//检测是否在地面（射线检测）
            //isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, layer);//检测是否在地面（老方法）

            if (!isHit && !isAttacking && !isDashing)
            {
                Jump();
                Move();
            }

            if (!isDashing && !isHanging)
            {
                Attack();
            }

            if (Input.GetButtonDown("Dash") && !isHit && !isAttacking && !isHanging)
            {
                if (Time.time >= (lastDashTime + dashCD))//判断是否正在CD
                {
                    ReadyToDash();//准备Dash
                    anim.SetTrigger("dashing");
                    CameraShake.instance.Shake();//屏幕震动
                }
            }

            if (Input.GetButtonDown("Jump") && jumpCount > 0 && !Input.GetKey(KeyCode.S))
            {
                jumpPressed = true;
            }

            if (isHit)//受击弹开
            {
                rb.velocity = new Vector2(dir.x * hurtSpeed, rb.velocity.y);
            }
        }
        if(Input.GetKey(KeyCode.S) && Input.GetButton("Jump"))
        {
            canDown = true;
        }
        OneWayPlatformCheck();
    }

    void FixedUpdate()
    {
        if (!GameController.isGameOver)
        {
            Dash();
        }
    }

    void Hang()
    {
        float direction = transform.localScale.x;//玩家面朝向

        Vector3 playerPos = transform.position;

        Vector2 rayDir = new Vector2(direction, 0);

        RaycastHit2D headCheck = Physics2D.Raycast(new Vector2(playerPos.x + (0.33f * direction), playerPos.y + playerHeight), rayDir, grabDistance, layer);//头顶的Raycast
        RaycastHit2D eyeCheck = Physics2D.Raycast(new Vector2(playerPos.x + (0.33f * direction), playerPos.y + playerEyeHeight), rayDir, grabDistance, layer);//眼睛的Raycast
        RaycastHit2D hangCheck = Physics2D.Raycast(new Vector2(playerPos.x + topRaycast * direction, playerPos.y + playerHeight), Vector2.down, grabDistance, layer);//往下的Raycast

        #region RayDebug
        //if (!headCheck)
        //{
        //    Debug.DrawRay(new Vector3(playerPos.x + 0.33f * direction, playerPos.y + playerHeight, 0), rayDir, Color.green);
        //}
        //else Debug.DrawRay(new Vector3(playerPos.x + 0.33f * direction, playerPos.y + playerHeight, 0), rayDir, Color.red);
        //if (!eyeCheck)
        //{
        //    Debug.DrawRay(new Vector3(playerPos.x + 0.33f * direction, playerPos.y + playerEyeHeight, 0), rayDir, Color.green);
        //}
        //else Debug.DrawRay(new Vector3(playerPos.x + 0.33f * direction, playerPos.y + playerEyeHeight, 0), rayDir, Color.red);
        //if (!hangCheck)
        //{
        //    Debug.DrawRay(new Vector3(playerPos.x + topRaycast * direction, playerPos.y + playerHeight, 0), Vector2.down, Color.green);
        //}
        //else Debug.DrawRay(new Vector3(playerPos.x + topRaycast * direction, playerPos.y + playerHeight, 0), Vector2.down, Color.red);
        #endregion

        if (!isGround && rb.velocity.y < 0 && hangCheck && eyeCheck && !headCheck)//正在悬挂
        {
            Vector3 pos = transform.position;                  //悬挂位置修正//
            pos.x += ((eyeCheck.distance - 0.05f) * direction);//悬挂位置修正//
            pos.y -= (hangCheck.distance);                     //悬挂位置修正//
            transform.position = pos;                          //悬挂位置修正//        

            rb.bodyType = RigidbodyType2D.Static;//静止

            isHanging = true;

            anim.SetBool("hanging", true);
            anim.SetBool("idle", false);
            anim.SetBool("falling", false);
        }
    }//悬挂

    void IsGroundCheck()//地面检测（射线
    {
        RaycastHit2D leftCheck = Physics2D.Raycast(new Vector2(transform.position.x - 0.34f, transform.position.y - 1), Vector2.down, 0.1f, layer);
        RaycastHit2D rightCheck = Physics2D.Raycast(new Vector2(transform.position.x + 0.34f, transform.position.y - 1), Vector2.down, 0.1f, layer);
        if (leftCheck || rightCheck || coll.IsTouchingLayers(oneWay))
        {
            isGround = true;
        }
        else isGround = false;
        if(coll.IsTouchingLayers(oneWay))//是否在单向平台（控制单向平台的向下功能
        {
            isOnOneWay = true;
        }
        #region RayDebug
        //if (!leftCheck)
        //{
        //    Debug.DrawRay(new Vector3(transform.position.x - 0.34f, transform.position.y - 1, 0), Vector2.down, Color.green);
        //}
        //else Debug.DrawRay(new Vector3(transform.position.x - 0.34f, transform.position.y - 1, 0), Vector2.down, Color.red);
        //if (!rightCheck)
        //{
        //    Debug.DrawRay(new Vector3(transform.position.x + 0.34f, transform.position.y - 1, 0), Vector2.down, Color.green);
        //}
        //else Debug.DrawRay(new Vector3(transform.position.x + 0.34f, transform.position.y - 1, 0), Vector2.down, Color.red);
        #endregion
    }

    void OneWayPlatformCheck()
    {
        if(isOnOneWay && canDown)
        {
            gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
            StartCoroutine(OneWayBack(0.3f));
        }
        
    }//单向平台效果（更改Layer

    IEnumerator OneWayBack(float time)//协程更改玩家越下单向平台后的Layer
    {
        yield return new WaitForSeconds(time);
        canDown = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void GetDamage(float damage,Vector2 direction)//受伤（用来外部调用
    {
        isHit = true;
        dir = -direction;
        CurrentHealth -= damage;
        ScreenFlash.instance.Flash();//屏幕闪烁
        CameraShake.instance.Shake();//屏幕震动
        Flash(flashTime);//人物闪烁
        anim.SetTrigger("hurting");
        Instantiate(bloodFX, transform.position, Quaternion.identity);  //BloodFXIns
    }

    void Attack()
    {
        if(Input.GetButtonDown("Attack") && !isAttacking && !isHit)
        {  
            isAttacking = true;
            attackCombo++;
            if (!isGround)//空中Attack***************************************TBD
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            if (attackCombo > 3)//Combo
            {
                attackCombo = 1;
            }
            timer = interval;
            anim.SetTrigger("attacking");
            anim.SetInteger("combo", attackCombo);
        }
        if(isAttacking)
        {
            rb.velocity = new Vector2(transform.localScale.x * attackSpeed * Time.fixedDeltaTime, rb.velocity.y);//速度补偿
        }
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
                attackCombo = 0;
            }
        }
        if (!isAttacking && !isGround)
        {
            rb.gravityScale = 5;
        }
    }

    void Move()
    {
        if (isHanging)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2") && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack3"))
        {
            rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);  //攻击动画播放完后才能移动
        }
        if (horizontal == 0 || !isGround)//判断是否移动
        {
            anim.SetInteger("running", 0);
        }
        else if (horizontal != 0)
        {
            anim.SetBool("idle", false);
            anim.SetInteger("running", 1);
            //AudioManager.instance.PlayAudioClip(index);
        }
        if (horizontal != 0 && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack") && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack2") && !anim.GetCurrentAnimatorStateInfo(1).IsName("Attack3"))
        {
            transform.localScale = new Vector3(horizontal, 1, 1);//面朝向（攻击动画播放完后才能转向）
        }
    }

    void Jump()
    {
        if(isHanging)
        {
            if(Input.GetButtonDown("Jump"))
            {
                rb.bodyType = RigidbodyType2D.Dynamic;//恢复重力
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);
                isHanging = false;
                anim.SetBool("jumping", true);
                anim.SetBool("hanging", false);
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                rb.bodyType = RigidbodyType2D.Dynamic;//恢复重力
                isHanging = false;
                anim.SetBool("hanging", false);
            }
        }

        if (isGround)
        {
            jumpCount = 1;                  //可修改跳跃次数（多段跳）
        }
  
        if(jumpPressed && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            jumpPressed = false;
            jumpCount--;
        }

        if (!isGround && rb.velocity.y < 0)
        {
            jumpCount = 0;
        }
        //若多段跳，增加判断即可
    }

    void ReadyToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDashTime = Time.time;
        UIManager.instance.InitEnergyBar();
    }

    void Dash()
    {
        if(isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x * Time.deltaTime, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ObjectPool.instance.GetFromPool();
            }
            if(dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }

    void Anim()
    {
        if (isGround)
        {
            anim.SetBool("idle", true);
            anim.SetBool("jumping", false);
            anim.SetBool("falling", false);
        }
        if(!isGround && rb.velocity.y>0)
        {
            anim.SetBool("idle", false);
            anim.SetBool("jumping", true);
        }
        else if(!isGround && rb.velocity.y<0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }





}
