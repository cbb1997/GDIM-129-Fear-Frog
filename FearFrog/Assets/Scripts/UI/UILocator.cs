using UnityEngine;

public class UILocator : MonoBehaviour
{
    public static UILocator Instance { get; private set; }
    public InventoryUI inventoryUI { get; private set; }
    public NotifsUI notifUI { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        GameObject uiObject = GameObject.FindWithTag("UI");
        inventoryUI = uiObject.GetComponent<InventoryUI>();
        notifUI = uiObject.GetComponent<NotifsUI>();
    }
}
