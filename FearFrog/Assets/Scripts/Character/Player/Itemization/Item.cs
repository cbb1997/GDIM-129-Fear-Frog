using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData m_ItemData;

    public static Action<ItemData> OnAcquired;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnAcquired?.Invoke(m_ItemData);
    }
}
