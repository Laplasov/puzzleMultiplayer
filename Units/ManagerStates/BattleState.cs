using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleState : IUnitManagerState
{
    public IUnitTransformer UTransform { set; get; }

    //SpaceMark m_currentUnit = null;
    //PlacementSystem m_currentBoard;
    PlacementValidator m_placementValidator;
    CellCalculator m_cellCalculator;
    //Vector2Int SingeSize = new Vector2Int(1, 1);
    public void Enter(PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer Transform)
    {
        m_placementValidator = placementValidator;
        m_cellCalculator = cellCalculator;
        UTransform = Transform;
    }

    public void Execute(UnitManager unitManager, SpaceMark target, PlacementSystem board) => ChooseTarget(unitManager, target, board);

    public void Exit(UnitManager unitManager)
    {

        var state = new PlacementState();
        state.Enter(m_placementValidator, m_cellCalculator, UTransform);
        unitManager.State = state;
    }

    void ChooseTarget(UnitManager unitManager, SpaceMark target, PlacementSystem board)
    { 

    }

}
