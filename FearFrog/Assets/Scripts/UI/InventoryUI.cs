using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image[] inventoryBox;
    public GameObject inventory;
    [SerializeField] private InventoryData m_InventoryData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            showItems();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void showItems()
    {
        ItemDataClass[] dataClass = m_InventoryData.GetItemDataClasses();
        DisplayItems(dataClass);
        Debug.Log(dataClass);
    }

    public void DisplayItems(ItemDataClass[] dataClasses)
    {
        for (int i = 0; i < dataClasses.Length; i++)
        {
            if (dataClasses[i] != null)
            {
                inventoryBox[i].sprite = dataClasses[i].Icon;
            }
        }
    }

}
