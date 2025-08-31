using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridBuildResult
{
    public Dictionary<Vector3, SpaceMark> GridMarks { get; }
    public Dictionary<Vector2Int, SpaceMark> GridMarksDimension { get; }
    public Func<Vector3, Vector3> CellPositionCalculator { get; }

    public GridBuildResult(
        Dictionary<Vector3, SpaceMark> gridMarks,
        Dictionary<Vector2Int, SpaceMark> gridMarksDimension,
        Func<Vector3, Vector3> cellPositionCalculator)
    {
        GridMarks = gridMarks;
        GridMarksDimension = gridMarksDimension;
        CellPositionCalculator = cellPositionCalculator;
    }
}
