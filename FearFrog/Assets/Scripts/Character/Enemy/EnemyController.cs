using UnityEngine;

public enum EnemyState 
{ 
    Inactive = 0,
    Idle = 1,
    Alert = 2,
    Aggressive = 3,
    Attacking = 4,
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData m_EnemyData;
    [SerializeField] private Animator m_Animator;

    private EnemyState m_CurrentState;

    private void Start()
    {
        //SetCurrentState(EnemyState.Idle);
    }

    private void Update()
    {

    }

    private void SetCurrentState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Inactive:
                InactiveBehavior();
                break;
            case EnemyState.Idle:
                IdleBehavior();
                break;
            case EnemyState.Alert:
                AlertBehavior();
                break;
            case EnemyState.Aggressive:
                AlertBehavior();
                break;
            case EnemyState.Attacking:
                AttackBehavior();
                break;
            default:
                break;
        }

        m_CurrentState = state;
        SetAnimState();
    }

    private void InactiveBehavior() { }
    private void IdleBehavior() { }
    private void AlertBehavior() { }
    private void AggressiveBehavior() { }
    private void AttackBehavior() { }

    private void SetAnimState() {
        m_Animator.SetInteger("AnimState", (int) m_CurrentState);
    }

    public void Respawn() { }
    public void Kill() { }
}
