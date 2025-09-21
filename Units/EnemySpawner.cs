using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    UnitManager m_unitManager;

    [SerializeField]
    private Vector2Int enemyCoordinates;

    [SerializeField]
    private UnitPrefabsSO unitPrefabsSO;

    [SerializeField]
    private string enemyUnitName;
    public List<string> AvailablePrefabNames
    {
        get
        {
            var names = new List<string>();
            if (unitPrefabsSO != null)
            {
                foreach (var prefab in unitPrefabsSO.GetAllPrefab())
                {
                    if (prefab != null)
                        names.Add(prefab.name);
                }
            }
            return names;
        }
    }
    [Button("Create Enemy")]
    public void CreateEnemyAtCoordinate()
    {
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        SpaceMark targetMark = grid[enemyCoordinates];
        m_unitManager.CreateUnit(targetMark, enemyUnitName, Owner.Enemy);
    }
}
