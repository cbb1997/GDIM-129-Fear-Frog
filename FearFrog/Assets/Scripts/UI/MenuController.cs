using UnityEngine;
using System;

public class MenuController : MonoBehaviour
{
    public static Action OnStartInitialized;
    public static Action OnQuitInitialized;

    [SerializeField] private GameObject m_MainMenu;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void InitializeStart() 
    {
        OnStartInitialized?.Invoke();
        DisableMainMenu();
    }

    public void InitializeQuit()
    { 
        OnQuitInitialized?.Invoke();
    }

    private void DisableMainMenu() 
    {
        m_MainMenu.SetActive(false);
    }
}
