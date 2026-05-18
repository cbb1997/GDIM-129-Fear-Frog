using UnityEngine;
using System;

[System.Serializable]
public class AudioDataClass 
{
    [SerializeField] private AudioClip m_AmbientTrack;
    public AudioClip AmbientTrack { get { return m_AmbientTrack; } }

    [SerializeField] private AudioClip m_MainMenuTrack;
    public AudioClip MainMenuTrack { get { return MainMenuTrack; } }

    [Range(0, 1.0f)]
    [SerializeField] private float m_Volume;
    public float Volume { get { return m_Volume; } }
}

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioDataClass m_DataClass;
    public AudioDataClass DataClass { get { return m_DataClass; } }

}
