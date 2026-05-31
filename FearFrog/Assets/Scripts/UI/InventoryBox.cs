using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryBox : MonoBehaviour
{
    [HideInInspector] public string itemName;

    public void SelectItem()
    {
        UILocator.Instance.inventoryUI.ShowItemDesc(itemName);
    }
}
