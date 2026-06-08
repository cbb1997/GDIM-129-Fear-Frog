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
    [SerializeField] private GameObject m_HUD;
    [SerializeField] private GameObject m_GameOverScreen;

    public static Action<GameState> OnGameStateChanged;

    private GameState m_CurrentState;

    private static GameController instance;

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
        m_HUD.SetActive(true);
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

    public static void EndGame()
    {
        if (instance == null) return;

        instance.m_GameOverScreen.SetActive(true);

        Time.timeScale = 0f;

        instance.SetGameState(GameState.Over);
    }

    private void Awake()
    {
        instance = this;
    }
}
