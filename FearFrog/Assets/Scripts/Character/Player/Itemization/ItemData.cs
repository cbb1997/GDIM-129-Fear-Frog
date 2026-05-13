using UnityEngine;
using System;

[System.Serializable]
public class ItemDataClass 
{
    [SerializeField] private string m_Name;
    public string Name { get { return m_Name; } }
    [SerializeField] private Sprite m_Icon;
    public Sprite Icon { get { return m_Icon; } }
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemDataClass m_DataClass;
    public ItemDataClass DataClass { get { return m_DataClass; } }
}
