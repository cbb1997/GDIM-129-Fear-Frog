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

    private GameObject[] m_PatrolTargets;
    private int m_PatrolIndex;

    private void Start()
    {
        GameController.OnGameStateChanged += GameStateListener;
        InitPatrol();
    }

    private void Update()
    {
        //UpdateCurrentState();
        SetCurrentState(EnemyState.Aggressive);
    }

    #region Helpers

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
        m_Animator.SetInteger("AnimState", (int)m_CurrentState);
    }

    #endregion

    #region State Machine

    private void UpdateCurrentState()
    {
        if (m_CurrentState == EnemyState.Inactive) return;

        int playerMeter = DetectPlayer();

        if (playerMeter > m_EnemyData.DataClass.AggroThreshold)
        {
            SetCurrentState(EnemyState.Aggressive);
        }
       
        SetCurrentState(EnemyState.Alert);
    }

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
                AggressiveBehavior();
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
    
    /*
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "PatrolTarget")
        {
            UpdatePatrolTarget();
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PatrolTarget")
        {
            UpdatePatrolTarget();
        }
    }
}
