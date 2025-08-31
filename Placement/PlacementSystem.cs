using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour, IMouseHover, IMouseSelect
{
    [SerializeField]
    GameObject _cellIndicator;
    
    [SerializeField]
    GameObject _cellMark;
    
    [SerializeField]
    private GridConfig _config;

    GridBuilder gridBuilder;
    Grid _grid;

    public Dictionary<Vector3, SpaceMark> GridMarks; 
    public Dictionary<Vector2Int, SpaceMark> GridMarksDimension; 
    public Func<Vector3, Vector3> CellPositionCalculation;

    void Awake()
    {
        InitializeComponents();
        BuildGrid();
        GridRegistry.RegisterGrid(_config.currentType, GridMarksDimension);
    }

    void InitializeComponents()
    {
        _grid = GetComponent<Grid>();
        _cellIndicator.transform.localScale = _config.GridToLocalScale(_grid);
        GridMarks = new Dictionary<Vector3, SpaceMark>();
    }

    private void BuildGrid()
    {
        gridBuilder = new GameObject("Builder")
            .AddComponent<GridBuilder>()
            .SetGridBuilder(_config, _grid, _cellMark);

        var buildResult = gridBuilder
            .SetWorldOffset()
            .PopulateGridMarks()
            .SetupMirrorMapping()
            .Build();

        GridMarks = buildResult.GridMarks;
        GridMarksDimension = buildResult.GridMarksDimension;
        CellPositionCalculation = buildResult.CellPositionCalculator;
    }

    public void OnMouseHover(Vector3 mouseWorldPosition)
    {
        Vector3 cellCenter = CellPositionCalculation(mouseWorldPosition);
        _cellIndicator.transform.position = cellCenter + Vector3.up * _config.indicatorOffset;
    }

    public SpaceMark OnMouseSelect(Vector3 mouseWorldPosition, out bool canInstantiate)
    {
        canInstantiate = CanInstantiate();
        Vector3 cellCenter = CellPositionCalculation(mouseWorldPosition);
        return GridMarks[cellCenter];
    }
    bool CanInstantiate() => _config.canInstantiate;
}
