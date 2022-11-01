using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> shadowPool = new Queue<GameObject>();//Ӱ�ӳ�

    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();//�����


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        FillPool();//��ʼ�������
    }
    
    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            var shadowIns = Instantiate(shadowPrefab);
            shadowIns.transform.SetParent(transform);

            ReturnPool(shadowIns);//ȡ�����ò����ض����
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);

        shadowPool.Enqueue(gameObject);
    }

    public GameObject GetFromPool()
    {
        if(shadowPool.Count == 0)//��������������
        {
            FillPool();
        }
        var shadowIns = shadowPool.Dequeue();

        shadowIns.SetActive(true);

        return shadowIns;
    }
}
