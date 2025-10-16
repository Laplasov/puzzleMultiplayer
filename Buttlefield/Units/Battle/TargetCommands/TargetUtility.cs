using UnityEngine;
using System.Collections.Generic;

public static class TargetUtility
{
    public static SpaceMark[] GetEnemiesInRow(Dictionary<Vector2Int, SpaceMark> grid, int row, Owner myOwner, TargetScope scope, int unitColumn, int unitSizeX)
    {
        List<SpaceMark> enemies = new List<SpaceMark>();

        switch (scope)
        {
            case TargetScope.Full:
                for (int col = 0; col <= 5; col++)
                {
                    CheckPositionForEnemies(grid, row, col, myOwner, enemies);
                }
                break;

            case TargetScope.One:
                CheckPositionForEnemies(grid, row, unitColumn, myOwner, enemies);
                for (int i = 1; i < unitSizeX; i++)
                    CheckPositionForEnemies(grid, row, unitColumn + i, myOwner, enemies);
                break;

            case TargetScope.None:
                break;
            case TargetScope.FirstThree:
            default:
                CheckPositionForEnemies(grid, row, unitColumn - 1, myOwner, enemies);
                CheckPositionForEnemies(grid, row, unitColumn, myOwner, enemies);
                CheckPositionForEnemies(grid, row, unitColumn + 1, myOwner, enemies);
                for (int i = 1; i < unitSizeX; i++)
                    CheckPositionForEnemies(grid, row, unitColumn + i, myOwner, enemies);
                break;
        }

        return enemies.ToArray();
    }

    public static bool IsRowBlocked(Dictionary<Vector2Int, SpaceMark> grid, int row, Owner myOwner, TargetScope blockScope, int unitColumn, int unitSizeX)
    {
        switch (blockScope)
        {
            case TargetScope.Full:
                for (int col = 0; col <= 5; col++)
                {
                    if (IsPositionBlockedByAlly(grid, row, col, myOwner))
                        return true;
                }
                break;

            case TargetScope.One:
                if (IsPositionBlockedByAlly(grid, row, unitColumn, myOwner))
                    return true;

                for (int i = 1; i < unitSizeX; i++)
                    if (IsPositionBlockedByAlly(grid, row, unitColumn + i, myOwner))
                        return true;
                break;

            case TargetScope.None:
                return false;
            case TargetScope.FirstThree:
            default:
                if (IsPositionBlockedByAlly(grid, row, unitColumn - 1, myOwner) ||
                    IsPositionBlockedByAlly(grid, row, unitColumn, myOwner) ||
                    IsPositionBlockedByAlly(grid, row, unitColumn + 1, myOwner))
                    return true;

                for (int i = 1; i < unitSizeX; i++)
                    if (IsPositionBlockedByAlly(grid, row, unitColumn + i, myOwner))
                        return true;
                break;
        }

        return false;
    }

    private static void CheckPositionForEnemies(Dictionary<Vector2Int, SpaceMark> grid, int row, int col, Owner myOwner, List<SpaceMark> enemies)
    {
        if (col < 0 || col > 5) return;

        Vector2Int pos = new Vector2Int(col, row);
        if (grid.TryGetValue(pos, out SpaceMark mark) && mark.Unit != null)
        {
            var unitStats = mark.Unit.GetComponent<UnitStats>();

            if (unitStats.IsDead == true) return;

            if (unitStats.Ownership != myOwner && !enemies.Contains(mark))
                    enemies.Add(mark);
        }
    }

    private static bool IsPositionBlockedByAlly(Dictionary<Vector2Int, SpaceMark> grid, int row, int col, Owner myOwner)
    {
        if (col < 0 || col > 5) return false;

        Vector2Int pos = new Vector2Int(col, row);
        if (grid.TryGetValue(pos, out SpaceMark mark) && mark.Unit != null)
        {
            var unitStats = mark.Unit.GetComponent<UnitStats>();
            if (unitStats.IsDead == true) 
                return false;
            return unitStats.Ownership == myOwner;
        }

        return false;
    }
}