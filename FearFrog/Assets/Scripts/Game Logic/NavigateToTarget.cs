using UnityEngine;
using UnityEngine.AI;

public class NavigateToTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_Agent;
    [SerializeField] private GameObject m_Target;

    private void Update()
    {
        m_Agent.SetDestination(m_Target.transform.position);
    }
}
