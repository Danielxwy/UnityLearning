using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioSource player;
    public List<AudioClip> audioClips;

    public void PlayAudioClip(int index)
    {
        player.PlayOneShot(audioClips[index]);
    }

}
