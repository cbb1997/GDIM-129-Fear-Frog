using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class InventoryUI : MonoBehaviour
{
    public Image[] inventoryIcon;
    public TMP_Text itemName;
    public GameObject inventory;
    [SerializeField] private InventoryData m_InventoryData;
    private ItemDataClass[] dataClass;
    public InventoryBox[] inventoryBoxes;

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
    }

    public void DisplayItems(ItemDataClass[] dataClasses)
    {
        for (int i = 0; i < dataClasses.Length; i++)
        {
            if (dataClasses[i] != null)
            {
                inventoryIcon[i].sprite = dataClasses[i].Icon;
                inventoryBoxes[i].itemName = dataClasses[i].Name;
            }
        }
    }

    public void ShowItemDesc(string name)
    {
        itemName.text = name;
    }

}
