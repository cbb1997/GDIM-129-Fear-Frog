using UnityEngine;

[CreateAssetMenu(fileName = "Debugger", menuName = "Scriptable Objects/Debugger")]
public class Debugger : ScriptableObject
{
    [SerializeField] private bool m_DisabledInBuild;

    public static void Log(string message) 
    {
#if UNITY_EDITOR || !m_DisabledInBuild
        Debug.Log(message);
#endif
    }

    public static void Warn(string message) 
    {
#if UNITY_EDITOR || !m_DisabledInBuild
        Debug.LogWarning(message);
#endif
    }

    public static void Error(string message) 
    {
#if UNITY_EDITOR || !m_DisabledInBuild
        Debug.LogError(message);
#endif
    }
}
