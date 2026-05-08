using UnityEngine;

[System.Serializable]
public class CharacterDataClass 
{
    [SerializeField] protected uint m_Health;
    public uint Health {  get { return m_Health; } }
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private CharacterDataClass m_DataClass;
    public CharacterDataClass DataClass { get { return m_DataClass; } }
}
