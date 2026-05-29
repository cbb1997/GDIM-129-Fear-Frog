using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryUI : MonoBehaviour
{
    public Image[] inventoryBox;
    public TMP_Text itemName;
    public GameObject inventory;
    [SerializeField] private InventoryData m_InventoryData;
    private ItemDataClass[] dataClass;

    private void Start()
    {
        InventoryData.OnAddItem += showItems;
    }

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
        dataClass = m_InventoryData.GetItemDataClasses();
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

    public void ShowItemDesc()
    {
        //itemName.text = 
    }

}
