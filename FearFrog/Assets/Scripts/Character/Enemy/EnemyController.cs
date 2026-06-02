using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

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
    
    [SerializeField] private float m_YOffset;

    [ReadOnly][SerializeField] private EnemyState m_CurrentState;

    public static Action<EnemyState, EnemyState> OnEnemyStateChange;

    private GameObject[] m_PatrolTargets;
    private int m_PatrolIndex;

    private void Start()
    {
        GameController.OnGameStateChanged += GameStateListener;
        EnemyController.OnEnemyStateChange += EnemyStateListiner;
        InitPatrol();
    }

    private void Update()
    {
        UpdateCurrentState();
        ExecuteStateBevaior();
    }

    #region Helpers

    private int StateAsInt()
    {
        return (int)m_CurrentState;
    }

    // Returns at integer value indicating an "aggro meter"
    private int DetectPlayer() 
    {
        RaycastHit vision = DrawRay();
        if (vision.collider != null && vision.collider.gameObject.tag != "Player")
        {
            return 0;
        }

        return (int) (vision.distance * 2);
    }

    private float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, m_Player.transform.position);
    }

    // Draw a raycast and return any hits
    private RaycastHit DrawRay() 
    {
        RaycastHit hit;
        Physics.Raycast(OffestPosition(), transform.TransformDirection(Vector3.forward), out hit, m_EnemyData.DataClass.SightDistance);
        Debug.DrawRay(OffestPosition(), transform.TransformDirection(Vector3.forward) * m_EnemyData.DataClass.SightDistance, Color.red);
        return hit;
    }

    // Calculate the offest position used for drawing raycasts
    private Vector3 OffestPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + m_YOffset, transform.position.z);
    }

    private void UpdateLocation()
    {
        m_EnemyData.DataClass.Position = GetComponent<Transform>().position;
    }

    // Find patrol objects in the scene and select and random target
    private void InitPatrol()
    {
        m_PatrolTargets = GameObject.FindGameObjectsWithTag("PatrolTarget");

        if (m_PatrolTargets.Length <= 0)
        {
            m_PatrolIndex = -1;
            return;
        }

        m_PatrolIndex = new System.Random().Next(m_PatrolTargets.Length - 1);
    }

    // Change to a new patrol target
    private void UpdatePatrolTarget()
    {
        if (m_PatrolIndex == m_PatrolTargets.Length - 1)
        {
            m_PatrolIndex = 0;
        }
        else
        {
            ++m_PatrolIndex;
        }
    }

    private void SetAnimState()
    {
        m_Animator.SetInteger("AnimState", StateAsInt());
    }

    #endregion

    #region State Machine

    // Ensure enemy ai is diabled when the game is inactive
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

    private void EnemyStateListiner(EnemyState oldState, EnemyState newState)
    { }

    private void UpdateCurrentState()
    {
        if (m_CurrentState == EnemyState.Inactive) return;

        if (StateAsInt() > 1)
        {
            UpdateAggro();
        }
        else if (m_CurrentState == EnemyState.Idle)
        {
            UpdateIdle();
        }
    }

    private void UpdateAggro()
    {
        int playerMeter = DetectPlayer();

        if (playerMeter > m_EnemyData.DataClass.AggroThreshold && StateAsInt() < 3)
        {
            SetCurrentState(EnemyState.Aggressive);
        }
        else if (StateAsInt() > 2)
        {
            SetCurrentState(EnemyState.Alert);
        }
    }

    private void UpdateIdle()
    {
        if (GetPlayerDistance() > m_EnemyData.DataClass.ActivationDistance)
        {
            SetCurrentState(EnemyState.Alert);
        }
    }

    private void SetCurrentState(EnemyState state)
    {
        OnEnemyStateChange?.Invoke(m_CurrentState, state);
        m_CurrentState = state;
        SetAnimState();
    }

    private void ExecuteStateBevaior()
    {
        switch (m_CurrentState)
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
                AggressiveBehavior();
                break;
            case EnemyState.Attacking:
                AttackBehavior();
                break;
            default:
                break;
        }
    }

    private void InactiveBehavior() { }

    private void IdleBehavior() { }

    // If patrol targets exist, use the navmesh to path to the current target
    private void AlertBehavior() 
    {
        if (m_PatrolIndex < 0) return;

        m_Agent.SetDestination(m_PatrolTargets[m_PatrolIndex].transform.position);
    }

    private void AggressiveBehavior() 
    {
        m_Agent.SetDestination(m_Player.transform.position);
    }

    private void AttackBehavior() { }
    #endregion

    public void Respawn() { }
    public void Kill() { }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PatrolTarget")
        {
            UpdatePatrolTarget();
        }
    }
}
