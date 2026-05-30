using UnityEngine;
using System;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ItemData m_ItemData;

    public static event Action<ItemData> OnAcquired;

    private void OnMouseDown()
    {
        OnAcquired?.Invoke(m_ItemData);
        Destroy(gameObject);
    }
}
