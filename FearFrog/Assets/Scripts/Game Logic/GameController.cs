using UnityEngine;
using UnityEditor;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameData m_GameData;
    
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_StartingLevel;

    private void Start()
    {
        DontDestroyOnLoad(this);

        MenuController.OnStartInitialized += StartGame;
        MenuController.OnQuitInitialized += QuitGame;
    }

    private void StartGame()
    {
        m_StartingLevel.SetActive(true);
        m_Player.SetActive(true);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; 
        return;
#endif
        Application.Quit();
    }
}
