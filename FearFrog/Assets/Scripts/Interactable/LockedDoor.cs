using UnityEngine;
using System;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private InventoryData m_InventoryData;
    [SerializeField] private int m_KeyID;
    [SerializeField] private Animator m_Animator;

    public static Action OnFailedUnlock;
    
    private DoorState m_CurrentState;
    private bool m_Locked = true;

    private void OnMouseDown()
    {
        if (m_Locked && !m_InventoryData.CheckItems(m_KeyID))
        {
            OnFailedUnlock?.Invoke();
            return;
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
