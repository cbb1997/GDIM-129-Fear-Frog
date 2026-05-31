using UnityEngine;

[System.Serializable]
public class EnemyDataClass : CharacterDataClass
{
    [SerializeField] private Vector3 m_Position;
    public Vector3 Position { get { return m_Position; } set { m_Position = value; } }
    
    [SerializeField] private Vector3 m_RespawnPos;
    public Vector3 RespawnPos { get { return m_RespawnPos; } }

    [SerializeField] private int m_AggroThreshold, m_AttackThreshold;
    public int AggroThreshold { get { return m_AggroThreshold; } }
    public int AttackThreshold { get { return m_AttackThreshold; } }

    [SerializeField] private float m_AggroTime;
    public float AggroTime { get { return m_AggroTime; } }

    [SerializeField] private float m_SightDistance;
    public float SightDistance { get { return m_SightDistance; } }
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyDataClass m_DataClass;
    public EnemyDataClass DataClass { get { return m_DataClass; } }
}
