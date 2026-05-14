using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryData m_InventoryData;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
