using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.STP;

class GridBuilder : MonoBehaviour
{
    GridConfig _config;
    Vector3 _worldOffset;
    GameObject MarksParent;
    Grid _grid;
    GameObject _cellMark;
    Dictionary<Vector3, SpaceMark> GridMarks;
    Dictionary<Vector2Int, SpaceMark> GridMarksDimension;
    CellCalculator _cellCalculator;
    Vector2Int GridOffset => new Vector2Int(_config.dimensions.x / 2, _config.dimensions.y / 2);

    public GridBuilder SetGridBuilder(GridConfig config, Grid grid, GameObject cellMark, CellCalculator cellCalculator)
    {
        _config = config;
        _grid = grid;
        _cellMark = cellMark;
        GridMarks = new Dictionary<Vector3, SpaceMark>();
        GridMarksDimension = new Dictionary<Vector2Int, SpaceMark>();
        _cellCalculator = cellCalculator;

        _config.grid = _grid;

        return this;
    }

    public GridBuilder SetWorldOffset()
    {
        float offsetX = (_config.dimensions.x % 2 == 1) ? 0.5f : 0f;
        float offsetZ = (_config.dimensions.y % 2 == 1) ? 0.5f : 0f;
        if (_config.dimensions.x == 1)
            offsetX = -0.2f;
        if (_config.dimensions.y == 1)
            offsetZ = -0f;
        _worldOffset = new Vector3(offsetX, 0, offsetZ);

        _config.worldOffset = _worldOffset;

        return this;
    }

    public GridBuilder PopulateGridMarks()
    {
        MarksParent = new GameObject("GridMarks");
        Vector2Int offset = GridOffset;
        for (int x = 0; x < _config.dimensions.x; x++)
        {
            for (int y = 0; y < _config.dimensions.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x - offset.x, 0, y - offset.y);
                Vector3 worldPosition = _grid.GetCellCenterWorld(cellPosition) - _worldOffset;
                Vector3 position = _cellCalculator.CellPosition(worldPosition, _config);

                GridMarks[position] =
                    Instantiate(_cellMark, worldPosition, Quaternion.identity, MarksParent.transform)
                    .AddComponent<SpaceMark>();
                GridMarks[position].name = $"{_config.currentType.ToString()} - {x},{y}";
                GridMarks[position].Dimension = new Vector2Int(x, y);
                GridMarks[position].Config = _config;
                Vector2Int globalCoordinate = new Vector2Int(x, y);
                GridMarksDimension[globalCoordinate] = GridMarks[position];
                GridMarks[position].GridMarksDimension = GridMarksDimension;
                GridMarks[position].Type = _config.currentType;
            }
        }
        return this;
    }

    public GridBuilder SetupMirrorMapping()
    {
        GameObject hidedMarks;
        hidedMarks = new GameObject($"hidedMarks - {_config.currentType.ToString()}");
        hidedMarks.SetActive(false);
        foreach (SpaceMark mark in GridMarks.Values)
        {
            if (_config.canInstantiate)
            {
                Vector2Int mirrorDimension = new Vector2Int(mark.Dimension.x, mark.Dimension.y + 1);
                GameObject mirrorObj = new GameObject($"MirrorMark {mark.Dimension.x}-{mark.Dimension.y}");
                mirrorObj.SetActive(false);
                SpaceMark newMark = mirrorObj.AddComponent<SpaceMark>();
                newMark.transform.parent = hidedMarks.transform; 
                GridMarksDimension[mirrorDimension] = newMark;
                GridMarksDimension[mirrorDimension].MirroredMark = mark;
            }
            int mirrorY = (_config.dimensions.y - 1) - mark.Dimension.y;
            mark.MirroredMark = GridMarksDimension[new Vector2Int(mark.Dimension.x, mirrorY)];
        }
        return this;
    }

    public GridBuildResult Build()
    {
        return new GridBuildResult(
            GridMarks,
            GridMarksDimension
        );
    }
}