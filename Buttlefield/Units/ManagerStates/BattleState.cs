using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleState : IUnitManagerState
{
    public IUnitTransformer UTransform { set; get; }

    //SpaceMark m_currentUnit = null;
    //PlacementSystem m_currentBoard;
    PlacementValidator m_placementValidator;
    CellCalculator m_cellCalculator;
    UnitManager m_unitManager;
    //Vector2Int SingeSize = new Vector2Int(1, 1);
    public void Enter(UnitManager unitManager, PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer Transform)
    {
        m_placementValidator = placementValidator;
        m_cellCalculator = cellCalculator;
        UTransform = Transform;
        m_unitManager = unitManager;

        m_unitManager.ToggleUnits();
        m_unitManager.OnRound?.Invoke(true);
    }

    public void Execute(SpaceMark target, PlacementSystem board) => ChooseTarget(target, board);

    public void Exit()
    {

        var state = new PlacementState();
        state.Enter(m_unitManager, m_placementValidator, m_cellCalculator, UTransform);
        m_unitManager.State = state;
        m_unitManager.OnRound?.Invoke(false);

        if (m_unitManager.EnemySpawner.Index + 1 < m_unitManager.EnemySpawner.PreparedEnemiesSO.Length)
            m_unitManager.EnemySpawner.Index++;
        else
            m_unitManager.EnemySpawner.Index = 0;
    }

    void ChooseTarget(SpaceMark target, PlacementSystem board)
    { 

    }

}
