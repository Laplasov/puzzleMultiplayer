using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public enum PlacementRule { Full, UnitPlacement }

public class CellCalculator : MonoBehaviour
{
    public PlacementRule Rule { get; set; } = PlacementRule.Full;
    public Vector2Int UnitSize { get; set; } = new Vector2Int(1,1);

    [SerializeField]
    MarkStatus _markStatus;

    Highlighter _highlighter;
    private void Awake() => _highlighter = new Highlighter();

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
        else if(config.currentType == PlacementType.UnitHolder)
            return CellFullPosition(position, config);

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
        var unitSizeOffset = new Vector3(config.grid.cellSize.x * ((UnitSize.x - 1) * 0.25f), 0, config.grid.cellSize.z * ((UnitSize.y - 1) * 0.25f));

        config.gridPosition = config.grid.WorldToCell(position + config.worldOffset - unitSizeOffset);
        Vector2Int offset = config.GridOffset;

        config.gridPosition.x = Mathf.Clamp(config.gridPosition.x, -offset.x, config.dimensions.x - offset.x - 1);
        config.gridPosition.z = Mathf.Clamp(config.gridPosition.z, -offset.y, (config.dimensions.y / 2) - offset.y - 1);

        return config.grid.GetCellCenterWorld(config.gridPosition) - config.worldOffset;
    }

    public void SetHover(Vector3 mouseWorldPosition, GridConfig config, Dictionary<Vector3, SpaceMark> GridMarks, Dictionary<Vector2Int, SpaceMark> GridMarksDimension)
    {
        Vector3 cellCenter = CellPosition(mouseWorldPosition, config);

        if (!GridMarks.ContainsKey(cellCenter))
            return;

        var mark = GridMarks[cellCenter];

        var marks = new List<SpaceMark>();
        if (config.currentType == PlacementType.Battlefield)
        {
            var dimension = mark.Dimension;

            if (dimension.x + UnitSize.x > config.dimensions.x)
                dimension.x = config.dimensions.x - UnitSize.x;
            if (dimension.y + UnitSize.y > config.dimensions.y)
                dimension.y = config.dimensions.y - UnitSize.y;

            for (int x = 0; x < UnitSize.x; x++)
            {
                for (int z = 0; z < UnitSize.y; z++)
                {
                    var newMark = GridMarksDimension[dimension + new Vector2Int(x, z)];
                    marks.Add(newMark);
                }
            }
        }
        else
        {
            var newMark = GridMarksDimension[mark.Dimension];
            marks.Add(newMark);
        }
        _highlighter.SetHighlight(marks);
        _markStatus.SetShowStats(cellCenter, GridMarks);
    }
}

