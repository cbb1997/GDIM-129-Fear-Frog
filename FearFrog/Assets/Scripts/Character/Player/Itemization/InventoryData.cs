using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/InventoryData")]
public class InventoryData : ScriptableObject
{
    [SerializeField] private ItemData[] m_Items;
    public ItemData[] Items { get { return m_Items; } }

    public static event Action OnAddItem;

    public ItemDataClass[] GetItemDataClasses()
    {
        ItemDataClass[] dataClasses = new ItemDataClass[m_Items.Length];
        Debugger.Log(dataClasses.ToString());

        for (int i = 0; i < m_Items.Length; i++)
        {
            if(m_Items[i] != null)
            {
                dataClasses[i] = m_Items[i].DataClass;
            }
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
                OnAddItem?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool CheckItems(int itemID)
    {
        bool AllNull = true;
        for (int i = 0; i < m_Items.Length; i++)
        {
            if (m_Items[i] != null)
            {
                AllNull = false;
                break;
            }
        }

        if (AllNull == false)
        {
            foreach (ItemData itemData in m_Items)
            {
                if (itemData.DataClass.ID == itemID)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
