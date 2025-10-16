using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementState : IUnitManagerState
{
    public IUnitTransformer UTransform { set; get; }

    SpaceMark m_currentUnit = null;
    PlacementSystem m_currentBoard;
    PlacementValidator m_placementValidator;
    CellCalculator m_cellCalculator;
    Vector2Int SingeSize = new Vector2Int(1, 1);
    UnitManager m_unitManager;

    public void Enter(UnitManager unitManager, PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer Transform) 
    {
        m_placementValidator = placementValidator;
        m_cellCalculator = cellCalculator;
        UTransform = Transform;
        m_unitManager = unitManager;

        m_unitManager.ClearUnits();
        m_unitManager.StartCoroutine(DelayedEnemySpawn());
    }
    public void Execute(SpaceMark target, PlacementSystem board) => ChooseTarget(target, board);
    public void Exit() 
    {
        if (!m_unitManager.IsUnitsAlive(Owner.Ally))
        {
            Debug.Log("No units on field or alive!"); 
            return;
        }

        var state = new BattleState();
        state.Enter(m_unitManager, m_placementValidator, m_cellCalculator, UTransform);
        m_unitManager.State = state;
        m_currentUnit = null;
    }
    private IEnumerator DelayedEnemySpawn()
    {
        yield return new WaitForSeconds(3f);
        m_unitManager.EnemySpawner.CreateGroup();
    }

    void ChooseTarget(SpaceMark target, PlacementSystem board)
    {

        if (m_currentUnit != null)
        {
            bool shouldMove, shouldSwap;
            bool canPerformAction = m_placementValidator.CanPerformAction(m_currentUnit, target, board, out shouldMove, out shouldSwap);

            if (!canPerformAction)
                return;

            if (shouldMove)
                UTransform.MoveUnit(m_currentUnit, target, board, m_currentBoard);
            else if (shouldSwap)
                UTransform.SwapUnits(m_currentUnit, target, board, m_currentBoard);
            else
                return;

            m_currentUnit = null;
            m_cellCalculator.Rule = PlacementRule.Full;
            m_cellCalculator.UnitSize = SingeSize;
        }
        else if (target.Unit != null)
        {
            SpaceMark unitToSelect = m_placementValidator.GetUnitToSelect(target);

            IPlacementRule UnitRule = unitToSelect.Unit.GetComponent<IPlacementRule>();
            m_cellCalculator.Rule = UnitRule.GetPlacementRule();
            m_cellCalculator.UnitSize = UnitRule.GetSize();

            m_currentUnit = unitToSelect;
            m_currentBoard = board;
            UTransform.SelectUnit(m_currentUnit, board);
        }
    }
}
