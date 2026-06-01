using UnityEngine;

public class DebugOnCollide : MonoBehaviour
{
    [SerializeField] private string m_Message;

    private void OnTriggerEnter(Collider other)
    {
        Debugger.Log("Triggered: " + m_Message);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debugger.Log("Collision: " + m_Message);
    }
}
