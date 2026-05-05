using UnityEngine;
using System.Collections.Generic;

public class SaveController : MonoBehaviour
{
    private const string m_SaveName = "save_";

    private SaveFile m_CurrentSaveFile;
    private List<SaveFile> m_Saves;

    [SerializeField] private SaveBuilder m_SaveBuilder;

    private void Start()
    {
        m_Saves = new List<SaveFile>();

        CreateNewSave();
    }

    private void SelectSave(SaveFile saveFile)
    {
        m_CurrentSaveFile = saveFile;
    }

    private void CreateNewSave()
    {
        SaveFile newSave = new SaveFile(m_SaveName + m_Saves.Count + 1);
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
