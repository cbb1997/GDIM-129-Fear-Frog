using UnityEngine;

[System.Serializable]
public class EnemyDataClass : CharacterDataClass
{
    [SerializeField] private Vector3 m_Location;
    public Vector3 Location { get { return m_Location; } set { m_Location = value; } }

    [SerializeField] private int m_AggroThreshold;
    public int AggroThreshold { get { return m_AggroThreshold; } }

    [SerializeField] private float m_AggroTime;
    public float AggroTime { get { return m_AggroTime; } }
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyDataClass m_DataClass;
    public EnemyDataClass DataClass { get { return m_DataClass; } }
}
