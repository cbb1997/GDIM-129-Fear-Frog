using UnityEngine;
using System;

[System.Serializable]
public class PlayerDataClass : CharacterDataClass
{
    // Player movement
    [SerializeField] private float m_walkAcceleration = 30f;
    public float WalkAcceleration { get { return m_walkAcceleration; } }
    [SerializeField] private float m_sprintAcceleration = 45f;
    public float SprintAcceleration { get { return m_sprintAcceleration; } }
    [SerializeField] private float m_crouchAcceleration = 18f;
    public float CrouchAcceleration { get { return m_crouchAcceleration; } }
    [SerializeField] private float m_cameraSensitivity = 100f;
    public float CameraSensitivity { get { return m_cameraSensitivity; } }
    [SerializeField] private float m_jumpAcceleration = 320f;
    public float JumpAcceleration { get { return m_jumpAcceleration; } }
    
    // Player camera bob
    [SerializeField] private bool m_bobEnabled = true;
    public bool BobEnabled { get { return m_bobEnabled; } }
}


[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private PlayerDataClass m_DataClass;
    public PlayerDataClass DataClass { get { return m_DataClass; } }
}
