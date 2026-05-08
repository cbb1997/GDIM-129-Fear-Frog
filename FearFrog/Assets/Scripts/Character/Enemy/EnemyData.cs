using UnityEngine;

[System.Serializable]
public class EnemyDataClass : CharacterDataClass { }

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private EnemyDataClass m_DataClass;
    public EnemyDataClass DataClass { get { return m_DataClass; } }
}
