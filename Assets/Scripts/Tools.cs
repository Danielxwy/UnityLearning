using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoSingleton<Tools>
{
    public Cinemachine.CinemachineCollisionImpulseSource cameraShake;

    public void CameraShake()
    {
        cameraShake.GenerateImpulse();
    }
}
