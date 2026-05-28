using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    [SerializeField] private NavMeshAgent m_Agent;

    [SerializeField] private GameObject m_Player;

    private EnemyState m_CurrentState;

    private void Start()
    {
        GameController.OnGameStateChanged += GameStateListener;
        //SetCurrentState(EnemyState.Aggressive);
    }

    private void Update()
    {
        UpdateCurrentState();
    }

    // Returns at integer value indicating an "aggro meter"
    private int DetectPlayer() 
    {
        return 0;
    }

    private void UpdateLocation()
    {
        m_EnemyData.DataClass.Position = GetComponent<Transform>().position;
    }

    #region State Machine

    private void UpdateCurrentState()
    {
        if (m_CurrentState == EnemyState.Inactive) return;

        int playerMeter = DetectPlayer();

        if (playerMeter > m_EnemyData.DataClass.AggroThreshold)
        {
            SetCurrentState(EnemyState.Aggressive);
        }
        else if (playerMeter > 0)
        {
            SetCurrentState(EnemyState.Alert);
        }
    }

    private void GameStateListener(GameState state)
    {
        switch (state)
        {
            case GameState.Active:
                SetCurrentState(EnemyState.Idle);
                break;
            default:
                SetCurrentState(EnemyState.Inactive);
                break;
        }
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

    private void AlertBehavior() 
    {
        m_Agent.SetDestination(GetPatrolTarget());
    }

    private Vector3 GetPatrolTarget() 
    {
        return Vector3.zero;
    }

    private void AggressiveBehavior() 
    {
        StartCoroutine(EngageAggro());
    }

    private IEnumerator EngageAggro()
    {
        m_Agent.SetDestination(m_Player.transform.position);

        yield return new WaitForSeconds(m_EnemyData.DataClass.AggroTime);
    }

    private void AttackBehavior() { }

    private void SetAnimState() {
        m_Animator.SetInteger("AnimState", (int) m_CurrentState);
    }
    #endregion

    public void Respawn() { }
    public void Kill() { }
}
