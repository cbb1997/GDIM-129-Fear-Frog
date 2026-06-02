using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum SoundType
{
    BREATHING,
    WALKING,
    ROAR,
    RELOAD,
    SHOOTING,
    PAIN,
    PICKUPKEY
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource ambientMusicSource;
    [SerializeField] private AudioSource chaseMusicSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //PlayAmbientMusic();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        if (instance == null) return;

        int index = (int)sound;
        
        if (index < 0 || index >= instance.soundList.Length) return;
        if (instance.soundList[index] == null) return;
        
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound],volume);
    }

    public static void PlayAmbientMusic()
    {
        if (instance == null) return;

        instance.chaseMusicSource.Stop();

        if (!instance.ambientMusicSource.isPlaying)
        {
            instance.ambientMusicSource.Play();
        }
    }

    public static void PlayChaseMusic()
    {
        if (instance == null) return;

        instance.ambientMusicSource.Stop();

        if (!instance.chaseMusicSource.isPlaying)
        {
            instance.chaseMusicSource.Play();
        }
    }
}
