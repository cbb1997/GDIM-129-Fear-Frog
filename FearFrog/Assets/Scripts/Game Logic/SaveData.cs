using UnityEngine;
using System;

[System.Serializable]
public class SaveBuilder
{
    [SerializeField] private PlayerData m_Player;
    public PlayerData Player { get { return m_Player; } }
    [SerializeField] private EnemyData m_Enemy;
    public EnemyData Enemy { get { return m_Enemy; } }
    [SerializeField] private GameData m_Game;
    public GameData Game { get { return m_Game; } }

    public SaveData BuildSave()
    {
        return new SaveData(m_Player.DataClass, m_Enemy.DataClass, m_Game.DataClass);
    }

    public void LoadInto(SaveData data)
    {

    }

    public static SaveData BuildEmptySave()
    {
        return new SaveData();
    }
}

[System.Serializable]
public class SaveData
{
    [SerializeField] private PlayerDataClass m_Player;
    public PlayerDataClass Player { get { return m_Player; } }
    [SerializeField] private EnemyDataClass m_Enemy;
    public EnemyDataClass Enemy { get { return m_Enemy; } }
    [SerializeField] private GameDataClass m_Game;
    public GameDataClass Game { get { return m_Game; } }

    [SerializeField] private string m_LastSaved;
    public string LastSaved { get { return m_LastSaved; } }

    public SaveData(PlayerDataClass player, EnemyDataClass enemy,
        GameDataClass game)
    {
        m_Player = player;
        m_Enemy = enemy;

        m_Game = game;
        
        LogSave();
    }

    public SaveData() { LogSave(); }

    public void LogSave()
    {
        m_LastSaved = $"{DateTime.UtcNow}";
    }
}
