using UnityEngine;

public class CenterRowTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        var unit = unitLogic.GetComponent<UnitStats>();
        var myOwner = unit.Ownership;
        var unitSizeX = unit.GetSize().x;
        var myPos = unitLogic.GetMyPosition();

        int targetRow = myOwner == Owner.Enemy ? 1 : 4;

        // Check if target row is blocked
        if (TargetUtility.IsRowBlocked(grid, targetRow, myOwner, config.TargetBlockEnum, myPos.x, unitSizeX))
        {
            return new SpaceMark[0];
        }

        return TargetUtility.GetEnemiesInRow(grid, targetRow, myOwner, config.TargetScopeEnum, myPos.x, unitSizeX);
    }
}