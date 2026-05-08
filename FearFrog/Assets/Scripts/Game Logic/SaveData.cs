using UnityEngine;
using System;

[System.Serializable]
public class SaveBuilder
{
    [SerializeField] private PlayerData m_Player;
    public PlayerData Player { get { return m_Player; } }
    [SerializeField] private GameData m_Game;
    public GameData Game { get { return m_Game; } }

    public SaveData BuildSave()
    {
        return new SaveData(m_Player.DataClass, m_Game.DataClass);
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
    [SerializeField] private GameDataClass m_Game;
    public GameDataClass Game { get { return m_Game; } }

    [SerializeField] private string m_LastSaved;
    public string LastSaved { get { return m_LastSaved; } }

    public SaveData(PlayerDataClass player, GameDataClass game)
    {
        m_Player = player;
        m_Game = game;
        
        LogSave();
    }

    public SaveData() { LogSave(); }

    public void LogSave()
    {
        m_LastSaved = $"{DateTime.UtcNow}";
    }
}
