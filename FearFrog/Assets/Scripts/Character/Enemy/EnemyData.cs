using UnityEngine;

[System.Serializable]
public class EnemyDataClass : CharacterDataClass
{
    [SerializeField] private Vector3 m_Position;
    public Vector3 Position { get { return m_Position; } set { m_Position = value; } }
    
    [SerializeField] private Vector3 m_RespawnPos;
    public Vector3 RespawnPos { get { return m_RespawnPos; } }

    [SerializeField] private float m_AggroThreshold, m_AttackThreshold;
    public float AggroThreshold { get { return m_AggroThreshold; } }
    public float AttackThreshold { get { return m_AttackThreshold; } }

    [SerializeField] private float m_ActivationDistance;
    public float ActivationDistance { get { return m_ActivationDistance; } }

    [SerializeField] private float m_SightDistance, m_HearDistance;
    public float SightDistance { get { return m_SightDistance; } }
    public float HearDistance { get { return m_HearDistance; } }
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyDataClass m_DataClass;
    public EnemyDataClass DataClass { get { return m_DataClass; } }
}
