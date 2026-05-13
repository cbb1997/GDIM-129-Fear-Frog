using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    [SerializeField] private ItemData[] m_Items;
    public ItemData[] Items { get { return m_Items; } }

    public ItemDataClass[] GetItemDataClasses()
    {
        ItemDataClass[] dataClasses = new ItemDataClass[m_Items.Length];

        for (int i = 0; i < m_Items.Length; i++)
        {
            dataClasses[i] = m_Items[i].DataClass;   
        }

        return dataClasses;
    }

    public bool AddItem (ItemData item) 
    {
        if (m_Items[m_Items.Length - 1] != null) return false;

        for (int i = 0; i < m_Items.Length ; i++) 
        {
            if (m_Items[i] == null) { 
                m_Items[i] = item;
                return true;
            }
        }

        return false;
    }
}
