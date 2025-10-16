using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    UnitManager m_unitManager;

    [SerializeField]
    public SceneDataBridgeSO SceneDataBridgeSO;
    public PreparedEnemiesSO[] PreparedEnemiesSO;

    public int Index {  get; set; }

    private void Start()
    {
        PreparedEnemiesSO = SceneDataBridgeSO.PreparedEnemiesSO;
    }

    [Button("Create Group")]
    public void CreateGroup()
    {
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        foreach (EnemyGroup enemy in PreparedEnemiesSO[Index].GroupOfEnemies)
        {
            SpaceMark targetMark = grid[enemy.position];
            m_unitManager.CreateUnit(targetMark, enemy.enemyUnit.name, Owner.Enemy);

        }
    }

}
