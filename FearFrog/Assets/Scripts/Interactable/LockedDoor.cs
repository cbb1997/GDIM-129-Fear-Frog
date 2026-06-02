using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private int m_DoorID;
    [SerializeField] private InventoryData m_InventoryData;
    [SerializeField] private string m_KeyName;
    [SerializeField] private Animator m_Animator;
    
    private DoorState m_CurrentState;
    private bool m_Locked;

    private void OnMouseDown()
    {
        if (m_Locked)
        {

        }

        SwitchState();
    }

    private void SwitchState()
    {
        if (m_CurrentState == DoorState.Closed) SetState(DoorState.Open);
        else if (m_CurrentState == DoorState.Open) SetState(DoorState.Closed);
    }

    private void SetState(DoorState state)
    {
        m_CurrentState = state;
        m_Animator.SetInteger("AnimState", (int)m_CurrentState);
    }

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
