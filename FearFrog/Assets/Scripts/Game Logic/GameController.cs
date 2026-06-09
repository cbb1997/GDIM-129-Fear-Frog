using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject m_HUD, m_MainMenu, m_GameOverScreen;

    public static Action<GameState> OnGameStateChanged;
    private static GameController instance;

    private GameState m_CurrentState;

    private void Start()
    {
        //DontDestroyOnLoad(this);

        MenuController.OnStartInitialized += StartGame;
        MenuController.OnQuitInitialized += QuitGame;
    }

    private void StartGame()
    {
        //m_StartingLevel.SetActive(true);

        if (m_Player == null)
        {
            m_Player = GameObject.FindWithTag("Player");
            Debugger.Log($"{m_Player}: {m_Player == null}");
            //m_Player = FindAnyObjectByType<PlayerController>().gameObject;
        }

        if (m_HUD == null)
        {
            m_HUD = GameObject.FindWithTag("HUD");
            //m_HUD = FindAnyObjectByType<InventoryUI>().gameObject;
        }

        m_Player.SetActive(true);
        m_HUD.SetActive(true);
        SetGameState(GameState.Active);
    }

    public void Restart() 
    {
        SceneManager.LoadScene(0);
    }

    private void SetGameState(GameState state)
    { 
        m_CurrentState = state;
        OnGameStateChanged?.Invoke(m_CurrentState);

        if (state != GameState.Active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

        instance.m_Player.SetActive(false);
        instance.m_HUD.SetActive(false);
        instance.m_GameOverScreen.SetActive(true);
        instance.SetGameState(GameState.Over);
    }

    private void Awake()
    {
        instance = this;
    }
}
