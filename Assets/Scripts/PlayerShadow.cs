using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;


    [Header("时间参数")]
    public float activeTime;//残影显示时间（持续时间）
    public float activeStart;//残影显示开始时间

    [Header("不透明度参数")]
    public float alphaInit = 1;//alpha初始值
    public float alphaMultiplier;
    private float alpha;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSprite = player.GetComponent<SpriteRenderer>();
        thisSprite = GetComponent<SpriteRenderer>();
        
        alpha = alphaInit;

        thisSprite.sprite = playerSprite.sprite;
        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }

    void FixedUpdate()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 0.5f, 1, alpha);

        thisSprite.color = color;

        if(Time.time >= (activeStart + activeTime))
        {
            ObjectPool.instance.ReturnPool(this.gameObject);//返回对象池
        }
    }
}
