using System;
using TMPro;
using Unity.Multiplayer.Playmode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject m_mainMenu;

    [SerializeField]
    GameObject m_levelSelect;

    [SerializeField]
    GameObject m_newGame;

    [SerializeField]
    GameObject m_loadGame;

    [SerializeField]
    GameObject m_setting;

    [SerializeField]
    GameObject m_chooseNameHolder;

    [SerializeField]
    TMP_InputField m_chooseNameInputField;

    [SerializeField]
    SceneAsset m_newScene;

    [SerializeField]
    LoadSlot[] m_loadSlotsNewGame;

    [SerializeField]
    LoadSlot[] m_loadSlotsLoadGame;

    string[] m_baseFileNames = new string[] { "File 1", "File 2", "File 3", "File 4" };
    string m_baseLastSave = "--/--/-- 00-00";
    string m_baseTotalTimePlay = "00:00:00";
    LoadSlot m_currentSlot;

    private void Start()
    {
        SetLoads(m_loadSlotsNewGame);
        SetLoads(m_loadSlotsLoadGame);
    }
    void SetLoads(LoadSlot[] loadSlots)
    {
        for (int i = 0; i < loadSlots.Length; i++)
        {
            SaveData slot = SaveManager.Instance.GetLoadSlot(i);
            if (slot == null)
            {
                loadSlots[i].FileName.text = m_baseFileNames[i];
                loadSlots[i].LastSave.text = m_baseLastSave;
                loadSlots[i].TotalTimePlay.text = m_baseTotalTimePlay;
            }
            else
            {
                loadSlots[i].FileName.text = slot.saveName;
                loadSlots[i].LastSave.text = slot.saveDateTime;
                loadSlots[i].TotalTimePlay.text = m_baseTotalTimePlay;
            }
        }
    }

    public void GoMainMenu()
    {
        m_mainMenu.SetActive(true);
        m_levelSelect.SetActive(false);
    }
    public void GoLevelSelect()
    {
        m_levelSelect.SetActive(true);
        m_mainMenu.SetActive(false);
    }

    public void LoadNewGame() => SceneManager.LoadScene(m_newScene.name);

    public void GoNewGame()
    {
        m_loadGame.SetActive(false);
        m_setting.SetActive(false);
        m_newGame.SetActive(true);
    }
    public void GoLoadGame()
    {
        m_setting.SetActive(false);
        m_newGame.SetActive(false);
        m_loadGame.SetActive(true);
    }
    public void GoSetting()
    {
        m_newGame.SetActive(false);
        m_loadGame.SetActive(false);
        m_setting.SetActive(true);
    }

    public void Exit() => Application.Quit();

    public void GoChoseName(LoadSlot slot)
    {
        m_chooseNameHolder.SetActive(true);
        m_currentSlot = slot;
    }

    public void ExitChoseName()
    {
        m_chooseNameHolder.SetActive(false);
        m_currentSlot = null;
        m_chooseNameInputField.text = "";
    }
    public void NewFile()
    {
        if (string.IsNullOrWhiteSpace(m_chooseNameInputField.text))
        {
            Debug.LogWarning("Please enter a save name!");
            return;
        }

        SaveManager.Instance.CreateSaveFile(m_currentSlot.Index, m_chooseNameInputField.text, m_newScene.name);
        ExitChoseName();
        SetLoads(m_loadSlotsNewGame);
        SetLoads(m_loadSlotsLoadGame);
        LoadNewGame();
    }
    public void DeleteAllSaves()
    {
        SaveManager.Instance.DeleteAllSaves();
        SetLoads(m_loadSlotsNewGame);
        SetLoads(m_loadSlotsLoadGame);
    }
    public void LoadGame(LoadSlot indexSlot) => SaveManager.Instance.LoadGame(indexSlot.Index);
}

