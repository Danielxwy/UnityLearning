using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFX : MonoBehaviour
{
    public float timeToDestroy;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        
    }

}
