using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int DoorID;
    [SerializeField] private InventoryData inventoryData;

    [Header ("TestStuff")]
    [SerializeField] private float pushForce = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        if (inventoryData.CheckItems(DoorID))
        {
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Find Key!");
        }

    }
}
