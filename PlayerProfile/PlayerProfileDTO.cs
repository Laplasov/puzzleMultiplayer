using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfileDTO
{
    public string playerName;
    public List<string> ownedUnitNames;
    public List<string> completedLevels;
    public PlayerProfileDTO()
    {
        ownedUnitNames = new List<string>();
        completedLevels = new List<string>();
    }

}
