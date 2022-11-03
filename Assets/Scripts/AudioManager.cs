using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource player;
    public List<AudioClip> audioClips;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlayAudioClip(int index)
    {
        player.PlayOneShot(audioClips[index]);
    }

}
