using UnityEngine;
using System.Collections.Generic;

public class SaveController : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "save_";
    private const string SAVE_FOLDER_NAME = "saves";

    private SaveFile m_CurrentSaveFile;
    private List<SaveFile> m_Saves;

    [SerializeField] private SaveBuilder m_SaveBuilder;

    private void Start()
    {
        DontDestroyOnLoad(this);
        Debugger.Log((string)Application.persistentDataPath);

        LoadSaveFiles();
        if (m_Saves == null) CreateNewSave();
    }

    private void LoadSaveFiles()
    {
        m_Saves = new List<SaveFile>(SaveFile.LoadSaves(SAVE_FOLDER_NAME));
    }

    private void SelectSave(SaveFile saveFile)
    {
        m_CurrentSaveFile = saveFile;
    }

    private void CreateNewSave()
    {
        SaveFile newSave = new SaveFile(SAVE_FOLDER_NAME, SAVE_FILE_NAME + m_Saves.Count + 1);
        m_Saves.Add(newSave);
        newSave.Save(m_SaveBuilder.BuildSave());
    }

    private void DeleteCurrentSave()
    {
        m_CurrentSaveFile.Delete();
        m_CurrentSaveFile = null;
    }

    private void SaveGame()
    {
        m_CurrentSaveFile.Save(m_SaveBuilder.BuildSave());
    }

    private void LoadGame()
    {
        m_SaveBuilder.LoadInto(m_CurrentSaveFile.Load());
    }
}
