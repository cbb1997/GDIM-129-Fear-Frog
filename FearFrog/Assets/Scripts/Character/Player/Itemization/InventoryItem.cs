using UnityEngine;
using System;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ItemData m_ItemData;
    public Image image;

    public static Action<ItemData> OnAcquired;

    public void InitialiseItem(ItemData item)
    {
        m_ItemData = item;
        image.sprite = item.DataClass.Icon;
    }


    private void OnTriggerEnter(Collider other)
    {
        OnAcquired?.Invoke(m_ItemData);
    }
}
