using UnityEngine;

public class LastRowTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        var myOwner = unitLogic.GetComponent<UnitStats>().Ownership;
        var myPos = unitLogic.GetMyPosition();

        int targetRow = myOwner == Owner.Enemy ? 0 : 5;

        // Check if target row is blocked
        if (TargetUtility.IsRowBlocked(grid, targetRow, myOwner, config.TargetBlockEnum, myPos.x))
        {
            return new SpaceMark[0];
        }


        return TargetUtility.GetEnemiesInRow(grid, targetRow, myOwner, config.TargetScopeEnum, myPos.x);
    }
}