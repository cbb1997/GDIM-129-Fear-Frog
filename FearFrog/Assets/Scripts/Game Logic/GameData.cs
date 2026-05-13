using UnityEngine;
using System;

[System.Serializable]
public class GameDataClass 
{
    [SerializeField] private uint m_CurrentLevelIndex;
    public uint CurrentLevelIndex { get { return m_CurrentLevelIndex; } set { m_CurrentLevelIndex = value; } }

    [SerializeField] private GameObject[] m_Levels;
    public GameObject[] Levels { get { return m_Levels; } }
}

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private GameDataClass m_DataClass;
    public GameDataClass DataClass { get { return m_DataClass; } }
}
