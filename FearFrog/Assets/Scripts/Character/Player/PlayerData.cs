using UnityEngine;
using System;

[System.Serializable]
public class PlayerDataClass : CharacterDataClass { }


[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private PlayerDataClass m_DataClass;
    public PlayerDataClass DataClass { get { return m_DataClass; } }
}
