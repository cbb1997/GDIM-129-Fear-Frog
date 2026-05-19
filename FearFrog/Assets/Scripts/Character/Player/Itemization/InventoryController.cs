using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryData m_InventoryData;

    public InventoryBox[] inventoryBox;
    public GameObject inventoryItemPrefab;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < inventoryBox.Length; i++)
        {
            InventoryBox box = inventoryBox[i];
            InventoryItem currentItem = box.GetComponentInChildren<InventoryItem>();
            if (currentItem == null)
            {
                CreateItem(item, box);
            }
        }
    }

    private void CreateItem(ItemData item, InventoryBox box)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, box.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

}
