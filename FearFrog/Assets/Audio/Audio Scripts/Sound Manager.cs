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

    private float ambientBaseVolume = 0.3f;
    private float chaseBaseVolume = 0.6f;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();

        ambientBaseVolume = ambientMusicSource.volume;
        chaseBaseVolume = chaseMusicSource.volume;
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

        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.FadeBackToAmbient());
    }

    public static void PlayChaseMusic()
    {
        if (instance == null) return;

        instance.StopAllCoroutines();
        
        instance.ambientMusicSource.Stop();

        if (!instance.chaseMusicSource.isPlaying)
        {
            instance.chaseMusicSource.volume = 0.8f;
            instance.chaseMusicSource.Play();
        }
    }

    private IEnumerator FadeBackToAmbient()
    {
        float duration = 3f;

        if (!ambientMusicSource.isPlaying)
        {
            ambientMusicSource.volume = 0f;
            ambientMusicSource.Play();
        }

        float startChaseVolume = chaseMusicSource.volume;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            chaseMusicSource.volume = Mathf.Lerp(startChaseVolume, 0f, t);
            ambientMusicSource.volume = Mathf.Lerp(0f, ambientBaseVolume, t);

            yield return null;
        }

        chaseMusicSource.Stop();

        chaseMusicSource.volume = chaseBaseVolume;
        ambientMusicSource.volume = ambientBaseVolume;
    }
}
