using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    public string Name;
    public PlayerProfileDTO Save;
    public List<GameObject> OwnedUnits = new List<GameObject>();
    private UnitPrefabsSO m_prefabs;
    public PlayerProfile(string name, UnitPrefabsSO prefabs, PlayerProfileDTO profileData = null)
    {
        Name = name;
        m_prefabs = prefabs;
        Save = profileData ?? new PlayerProfileDTO();
        Save.playerName = name;
        RefreshOwnedUnits();
    }

    public void RefreshOwnedUnits()
    {
        OwnedUnits.Clear();
        foreach (string unitName in Save.ownedUnitNames)
        {
            GameObject unitPrefab = m_prefabs.GetPrefabByName(unitName);
            if (unitPrefab != null)
                OwnedUnits.Add(unitPrefab);
        }
    }

    public void AddUnit(string unitName)
    {
        if (!Save.ownedUnitNames.Contains(unitName))
        {
            Save.ownedUnitNames.Add(unitName);
            GameObject unitPrefab = m_prefabs.GetPrefabByName(unitName);
            if (unitPrefab != null)
                OwnedUnits.Add(unitPrefab);
        }
    }

    public void RemoveUnit(string unitName)
    {
        Save.ownedUnitNames.Remove(unitName);
        GameObject unitPrefab = m_prefabs.GetPrefabByName(unitName);
        if (unitPrefab != null)
            OwnedUnits.Remove(unitPrefab);
    }

    public bool HasUnit(string unitName) => Save.ownedUnitNames.Contains(unitName);
}
