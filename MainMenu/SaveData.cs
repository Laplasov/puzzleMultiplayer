
using System;

[System.Serializable]
public class SaveData
{
    public string saveName;
    public string saveDateTime;
    public string sceneName;
    public PlayerProfileDTO playerProfileDTO;
    public SaveData() 
    {
        playerProfileDTO = new PlayerProfileDTO();
    }
    public SaveData(string name, string scene)
    {
        saveName = name;
        saveDateTime = DateTime.Now.ToString("MM/dd/yy HH-mm");
        sceneName = scene;
        playerProfileDTO = new PlayerProfileDTO();
    }
}