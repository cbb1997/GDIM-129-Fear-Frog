using UnityEngine;
using UnityEditor;
using System;

public enum GameState 
{
    Menu,
    Active,
    Paused,
    Over
}

public class GameController : MonoBehaviour
{
    [SerializeField] private GameData m_GameData;
    
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_StartingLevel;

    public static Action<GameState> OnGameStateChanged;

    private GameState m_CurrentState;

    private void Start()
    {
        DontDestroyOnLoad(this);

        MenuController.OnStartInitialized += StartGame;
        MenuController.OnQuitInitialized += QuitGame;
    }

    private void StartGame()
    {
        //m_StartingLevel.SetActive(true);
        m_Player.SetActive(true);
        SetGameState(GameState.Active);
    }

    private void SetGameState(GameState state)
    { 
        m_CurrentState = state;
        OnGameStateChanged?.Invoke(m_CurrentState);
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
