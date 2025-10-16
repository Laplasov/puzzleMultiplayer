using UnityEngine;

public class BackwardTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        Vector2Int myPos = unitLogic.GetMyPosition();
        var grid = GridRegistry.Instance.GetGrid(PlacementType.Battlefield);
        var unit = unitLogic.GetComponent<UnitStats>();
        var myOwner = unit.Ownership;
        var unitSizeX = unit.GetSize().x;

        // Backward means starting from enemy's front row and going to their back row
        int startRow = myOwner == Owner.Ally ? 5 : 0;  // Enemy's front row
        int endRow = myOwner == Owner.Ally ? 3 : 2;    // Enemy's back row
        int step = myOwner == Owner.Ally ? -1 : 1;     // Direction toward enemy's back row

        for (int row = startRow; myOwner == Owner.Ally ? row >= endRow : row <= endRow; row += step)
        {
            // Check if this row is blocked according to TargetBlockEnum
            if (TargetUtility.IsRowBlocked(grid, row, myOwner, config.TargetBlockEnum, myPos.x, unitSizeX))
            {
                return new SpaceMark[0]; // Blocked - stop search
            }

            // If not blocked, search for enemies according to TargetScopeEnum
            var enemies = TargetUtility.GetEnemiesInRow(grid, row, myOwner, config.TargetScopeEnum, myPos.x, unitSizeX);
            if (enemies.Length > 0)
            {
                return enemies;
            }
        }

        return new SpaceMark[0];
    }
}