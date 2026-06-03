using UnityEngine;
using System;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ItemData m_ItemData;

    public static event Action<ItemData> OnAcquired;

    private void OnMouseDown()
    {
        OnAcquired?.Invoke(m_ItemData);

        // KEY jingle
        SoundManager.PlaySound(SoundType.PICKUPKEY, 0.5f);
        
        Destroy(gameObject);
    }
}
