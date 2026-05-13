using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class SaveFile
{
    private const string BINARY_EXTENSION = ".dat";
    private const string RAW_EXTENSION = "json";

    private string m_DirectoryName, m_Filename, m_Extension;
    public string DirectoryName { get { return m_DirectoryName; } }
    public string Filename { get { return m_Filename; } }
    public string Extension { get { return m_Extension; } }

    private string m_SavePath;
    public string SavePath { get { return m_SavePath; } }

    public SaveFile(string directoryName, string filename, bool encrypt = false)
    {
        m_DirectoryName = directoryName;
        m_Filename = filename;

        if (encrypt) m_Extension = BINARY_EXTENSION;
        else m_Extension = RAW_EXTENSION;

        m_SavePath = Path.Combine(Application.persistentDataPath, m_DirectoryName, m_Filename + m_Extension);
    }

    public static List<SaveFile> LoadSaves(string directoryName)
    {
        List<SaveFile> saves = new List<SaveFile>();

        return saves;
    }

    public void Save(SaveData data) 
    {
        string json = JsonUtility.ToJson(data);
        WriteSave(json);
    }

    public SaveData Load() 
    {
        SaveData emptyData = SaveBuilder.BuildEmptySave();
        string json = ReadSave();
        JsonUtility.FromJsonOverwrite(json, emptyData);
        return emptyData;
    }

    public void Delete()
    {

    }

    private string ReadSave() 
    { 
        return ""; 
    }
    
    private void WriteSave(string json) 
    {
        Debugger.Log(json);
    }
    
}
