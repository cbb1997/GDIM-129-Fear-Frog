using UnityEngine;

public class TestTest : MonoBehaviour
{
    public InventoryController inventoryController;
    public ItemData items;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PickUpItem();
        }
    }

    public void PickUpItem()
    {
        inventoryController.AddItem(items);
    }
}
