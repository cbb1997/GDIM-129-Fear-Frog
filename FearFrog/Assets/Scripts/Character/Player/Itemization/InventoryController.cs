using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryData m_InventoryData;

    void Start()
    {
        DontDestroyOnLoad(this);
        InventoryItem.OnAcquired += AddItem;
    }

    public void AddItem(ItemData item)
    {
        m_InventoryData.AddItem(item);
    }

}
