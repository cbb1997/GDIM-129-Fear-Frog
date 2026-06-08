using UnityEngine;
using System;
using EditorInvokeButton;

[System.Serializable]
public class ItemDataClass 
{
    [SerializeField] private string m_Name;
    public string Name { get { return m_Name; } }
    [SerializeField] private Sprite m_Icon;
    public Sprite Icon { get { return m_Icon; } }
    [SerializeField] private int m_ID;
    public int ID { get { return m_ID; } }
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemDataClass m_DataClass;
    public ItemDataClass DataClass { get { return m_DataClass; } }
}
