using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Cinemachine.CinemachineCollisionImpulseSource cameraShake;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        cameraShake = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();
    }

    public void Shake()
    {
        cameraShake.GenerateImpulse();
    }

}
