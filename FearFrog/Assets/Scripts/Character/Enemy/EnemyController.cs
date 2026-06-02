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

    [SerializeField] private EnemyState m_StartingState;
    [ReadOnly][SerializeField] private EnemyState m_CurrentState;

    // Audio for Monster
    [SerializeField] private float roarDelay = 6f;
    private float roarTimer = 20f;

    public static Action<EnemyState, EnemyState> OnEnemyStateChange;

    private GameObject[] m_PatrolTargets;
    private int m_PatrolIndex;

    [ReadOnly][SerializeField] private float m_AggroTime;

    private void Start()
    {
        GameController.OnGameStateChanged += GameStateListener;
        EnemyController.OnEnemyStateChange += EnemyStateListiner;
        
        InitPatrol();
        SetCurrentState(m_StartingState);
    }

    private void Update()
    {
        UpdateCurrentState();
        ExecuteStateBevaior();

        roarTimer -= Time.deltaTime;

        if (roarTimer <= 0f)
        {
            SoundManager.PlaySound(SoundType.ROAR, 0.7f);
            roarTimer = UnityEngine.Random.Range(20f, 30f);
        }
    }

    #region Helpers

    private int StateAsInt()
    {
        return (int)m_CurrentState;
    }

    // "Sees" the player with a raycast
    private float DetectPlayer() 
    {
        RaycastHit vision = DrawRay();

        //Debugger.Log($"{vision.collider?.gameObject}");
        if (vision.collider?.gameObject.tag == "Player")
        {
            return vision.distance;
        }

        RaycastHit[] hearing = DrawSphere();

        foreach (var e in hearing)
        {
            if (e.collider?.gameObject.tag == "Player")
            {
                return GetPlayerDistance();
            }
        }

        return m_EnemyData.DataClass.SightDistance;
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

    // Draws a spherecast and returns any hits
    private RaycastHit[] DrawSphere()
    {
        return Physics.SphereCastAll(OffestPosition(), m_EnemyData.DataClass.HearDistance, transform.TransformDirection(Vector3.down), 0);
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
        float playerDistance = DetectPlayer();

        if (StateAsInt() > 2)
        {
            if (playerDistance < m_EnemyData.DataClass.SightDistance)
            {
                m_AggroTime += Time.deltaTime;
            }
            else
            {
                m_AggroTime -= Time.deltaTime;
            }

            if (m_AggroTime <= 0)
            {
                m_AggroTime = 0;
                SetCurrentState(EnemyState.Alert);
            }
        }
        else
        {
            // Trigger attack
            // if (playerDistance < m_EnemeyData.DataClass.AttackThreshold) { }

            if (playerDistance < m_EnemyData.DataClass.AggroThreshold)
            {
                SetCurrentState(EnemyState.Aggressive);
            }
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

    private void InactiveBehavior() 
    { }

    private void IdleBehavior() 
    { }

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
