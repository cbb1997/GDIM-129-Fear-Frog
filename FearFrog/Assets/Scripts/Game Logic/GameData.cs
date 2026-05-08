using UnityEngine;
using System;

[System.Serializable]
public class GameDataClass { }


[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private GameDataClass m_DataClass;
    public GameDataClass DataClass { get { return m_DataClass; } }
}
