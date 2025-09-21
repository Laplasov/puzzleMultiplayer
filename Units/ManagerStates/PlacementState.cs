using System;
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

    public void Enter(PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer Transform) 
    {
        m_placementValidator = placementValidator;
        m_cellCalculator = cellCalculator;
        UTransform = Transform;
    }
    public void Execute(UnitManager unitManager, SpaceMark target, PlacementSystem board) => ChooseTarget(unitManager, target, board);
    public void Exit(UnitManager unitManager) 
    {
        if (unitManager.IsUnitsAlive(Owner.Ally))
        {
            Debug.Log("No units on field or alive!"); 
            return;
        }

        var state = new BattleState();
        state.Enter(m_placementValidator, m_cellCalculator, UTransform);
        unitManager.State = state;
    }

    void ChooseTarget(UnitManager unitManager, SpaceMark target, PlacementSystem board)
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
