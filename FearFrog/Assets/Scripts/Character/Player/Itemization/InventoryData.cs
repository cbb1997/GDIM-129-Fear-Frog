using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    [SerializeField] private ItemData[] m_Items;
    public ItemData[] ItemDatas { get { return m_Items; } }

    public ItemDataClass[] GetItemDataClasses()
    {
        ItemDataClass[] dataClasses = new ItemDataClass[m_Items.Length];

        for (int i = 0; i < m_Items.Length; i++)
        {
            dataClasses[i] = m_Items[i].DataClass;   
        }

        return dataClasses;
    }
}
