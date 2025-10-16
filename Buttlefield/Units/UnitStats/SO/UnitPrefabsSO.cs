using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitPrefabsSO", menuName = "Scriptable Objects/UnitPrefabsSO")]
public class UnitPrefabsSO : ScriptableObject
{
    [SerializeField]
    private List<GameObject> m_Prefab;
    [SerializeField]
    private List<StatsModifier> m_StatsModifier;
    public GameObject GetPrefab(string propertyName) => 
        GetPrefabByName(propertyName);
    public List<GameObject> GetAllPrefab() => m_Prefab;
    public GameObject GetPrefabByName(string prefabName) => 
        m_Prefab?.FirstOrDefault(prefab => prefab != null && prefab.name == prefabName);
    public StatsModifier GetModifier(string modName) => 
        m_StatsModifier?.FirstOrDefault(mod => mod != null && mod.GetType().Name == modName);
    public GameObject GetPrefabByFieldName(string name) =>
        m_Prefab?.FirstOrDefault(prefab => prefab != null && prefab.GetComponent<UnitStats>().Name == name);
    public GameObject GetPrefabWithModifiers(string propertyName, params string[] modifierNames)
    {
        var prefab = GetPrefabByName(propertyName);
        if (prefab == null) 
        {
            Debug.Log($"Prefab {propertyName} not find in prefab collection.");
            return null;
        }

        var instance = Instantiate(prefab);
        var unitStats = instance.GetComponent<UnitStats>();

        if (unitStats != null && modifierNames.Length > 0)
        {
            foreach (var modName in modifierNames)
            {
                var modifier = GetModifier(modName);
                if (modifier != null)
                    unitStats.StatsModifier.Add(modifier);
            }
        }
        return instance;
    }
}
