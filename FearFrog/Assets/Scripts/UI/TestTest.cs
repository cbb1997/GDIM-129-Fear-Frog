using UnityEngine;

public class TestTest : MonoBehaviour
{
    public InventoryController inventoryController;
    public ItemData items;

    private void OnMouseDown()
    {
        PickUpItem();
    }

    public void PickUpItem()
    {
        inventoryController.AddItem(items);
    }
}
