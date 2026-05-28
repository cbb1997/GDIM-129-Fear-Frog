using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryBox : MonoBehaviour
{
    public ItemData itemData;

    public void SelectItem()
    {
        UILocator.Instance.inventoryUI.ShowItemDesc(itemData.DataClass);
    }
}
