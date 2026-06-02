using UnityEngine;

public enum DoorState
{
    Closed, Open
}

public class Door : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    private DoorState m_CurrentState;

    private void OnMouseDown()
    {
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
}
