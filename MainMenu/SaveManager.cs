using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveManager : MonoBehaviour
{
    protected SaveManager() {}
    public static SaveManager Instance { get; private set; }
    public SaveData CurrentPlayer { get; set; } = null;
    int m_currentIndex { get; set; }

    string[] m_FileNames = new string[] { "File1.save", "File2.save", "File3.save", "File4.save" };
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SaveData GetLoadSlot(int index)
    {
        string path = Path.Combine(Application.persistentDataPath, m_FileNames[index]);
        if (!File.Exists(path))
            return null;

        using (FileStream stream = new FileStream(path, FileMode.Open))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            string json = reader.ReadString();
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
    }
    public void SaveProgress()
    {
        CurrentPlayer.saveDateTime = DateTime.Now.ToString("MM/dd/yy HH-mm");
        CurrentPlayer.sceneName = SceneManager.GetActiveScene().name;
        string path = Path.Combine(Application.persistentDataPath, m_FileNames[m_currentIndex]);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            string json = JsonUtility.ToJson(CurrentPlayer);
            writer.Write(json);
        }
    }
    public void CreateSaveFile(int index, string name, string scene)
    {
        string path = Path.Combine(Application.persistentDataPath, m_FileNames[index]);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            SaveData data = new SaveData(name, scene);
            string json = JsonUtility.ToJson(data);
            writer.Write(json);
            CurrentPlayer = data;
            m_currentIndex = index;
        }
    }

    public void DeleteAllSaves()
    {
        for (int i = 0; i < m_FileNames.Length; i++)
        {
            string path = Path.Combine(Application.persistentDataPath, m_FileNames[i]);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
    public void LoadGame(int index)
    {
        SaveData data = GetLoadSlot(index);
        if (data != null)
        {
            CurrentPlayer = data;
            m_currentIndex = index;
            SceneManager.LoadScene(data.sceneName);
        }
    }
}
