using System.Data;
using UnityEngine;

public enum PlacementRule { Full, UnitPlacement }

public class CellCalculator : MonoBehaviour
{
    public PlacementRule Rule { get; set; } = PlacementRule.Full;

    public Vector3 CellPosition(Vector3 position, GridConfig config)
    {
        if (config.currentType == PlacementType.Battlefield)
            switch (Rule)
            {
                case PlacementRule.Full:
                    return CellFullPosition(position, config);
                case PlacementRule.UnitPlacement:
                    return CalculateUnitPlacement(position, config);
                default:
                    return CellFullPosition(position, config);
            }
        else 
            return CellFullPosition(position, config);
    }

    Vector3 CellFullPosition(Vector3 position, GridConfig config)
    {
        config.gridPosition = config.grid.WorldToCell(position + config.worldOffset);
        Vector2Int offset = config.GridOffset;
        config.gridPosition.x = Mathf.Clamp(config.gridPosition.x, -offset.x, config.dimensions.x - offset.x - 1);
        config.gridPosition.z = Mathf.Clamp(config.gridPosition.z, -offset.y, config.dimensions.y - offset.y - 1);
        return config.grid.GetCellCenterWorld(config.gridPosition) - config.worldOffset;
    }

    Vector3 CalculateUnitPlacement(Vector3 position, GridConfig config)
    {
        config.gridPosition = config.grid.WorldToCell(position + config.worldOffset);
        Vector2Int offset = config.GridOffset;

        config.gridPosition.x = Mathf.Clamp(config.gridPosition.x, -offset.x, config.dimensions.x - offset.x - 1);

        int maxZ = (config.dimensions.y / 2) - offset.y - 1;
        config.gridPosition.z = Mathf.Clamp(config.gridPosition.z, -offset.y, maxZ);

        return config.grid.GetCellCenterWorld(config.gridPosition) - config.worldOffset;
    }
}