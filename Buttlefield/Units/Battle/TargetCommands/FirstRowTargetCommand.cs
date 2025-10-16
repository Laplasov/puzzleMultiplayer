using System.Collections.Generic;
using UnityEngine;

public class FirstRowTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        Vector2Int myPos = unitLogic.GetMyPosition();
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        var unit = unitLogic.GetComponent<UnitStats>();
        var myOwner = unit.Ownership;
        var unitSizeX = unit.GetSize().x;

        int targetRow = myOwner == Owner.Enemy ? 2 : 3;
        int step = targetRow > myPos.y ? 1 : -1;

        return SearchRowsUntilTarget(grid, myPos, myOwner, targetRow, step, config, unitSizeX);
    }

    private SpaceMark[] SearchRowsUntilTarget(Dictionary<Vector2Int, SpaceMark> grid, Vector2Int myPos, Owner myOwner, int targetRow, int step, UnitCommandConfig config, int unitSizeX)
    {
        for (int row = myPos.y + step; row != targetRow + step; row += step)
        {
            // Check if this row is blocked
            if (TargetUtility.IsRowBlocked(grid, row, myOwner, config.TargetBlockEnum, myPos.x, unitSizeX))
            {
                return new SpaceMark[0]; // Blocked - stop search
            }

            // If not blocked, search for enemies
            var enemies = TargetUtility.GetEnemiesInRow(grid, row, myOwner, config.TargetScopeEnum, myPos.x, unitSizeX);
            if (enemies.Length > 0)
            {
                return enemies;
            }
        }

        return new SpaceMark[0];
    }
}