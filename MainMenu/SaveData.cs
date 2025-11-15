
using System;
using UnityEditor.Overlays;

[System.Serializable]
public class SaveData
{
    public string saveName;
    public string saveDateTime;
    public string sceneName;
    public SaveData() { }
    public SaveData(string name, string scene)
    {
        saveName = name;
        saveDateTime = DateTime.Now.ToString("MM/dd/yy HH-mm");
        sceneName = scene;
    }
}