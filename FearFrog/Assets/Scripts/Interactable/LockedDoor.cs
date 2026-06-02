using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private int m_DoorID;
    [SerializeField] private InventoryData m_InventoryData;
    [SerializeField] private string m_RequiredKeyName;

    /*
    private void OnMouseDown()
    {
        if (inventoryData.CheckItems(DoorID))
        {
            
        }
        else
        {
            UILocator.Instance.notifUI.ShowNotif(RequiredKeyName);
        }

    }
    */
}
