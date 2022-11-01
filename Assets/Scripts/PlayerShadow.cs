using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    private Transform player;

    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;

    private Color color;


    [Header("ʱ�����")]
    public float activeTime;//��Ӱ��ʾʱ�䣨����ʱ�䣩
    public float activeStart;//��Ӱ��ʾ��ʼʱ��

    [Header("��͸���Ȳ���")]
    public float alphaInit = 1;//alpha��ʼֵ
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
            ObjectPool.instance.ReturnPool(this.gameObject);//���ض����
        }
    }
}
