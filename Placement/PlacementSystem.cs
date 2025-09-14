using System.Collections.Generic;
using UnityEngine;


public class PlacementSystem : MonoBehaviour, IMouseHover, IMouseSelect
{
    [SerializeField]
    GameObject _cellIndicator;
    
    [SerializeField]
    GameObject _cellMark;
    
    [SerializeField]
    CellCalculator _cellCalculator;

    [SerializeField]
    GridConfig _config;

    [SerializeField]
    MarkStatus _markStatus;

    GridBuilder _gridBuilder;
    Grid _grid;
    Highlighter _highlighter;

    public Dictionary<Vector3, SpaceMark> GridMarks; 
    public Dictionary<Vector2Int, SpaceMark> GridMarksDimension; 
    public bool CanInstantiate() => _config.canInstantiate;
    public PlacementType GetPlacementType() => _config.currentType;

    void Awake()
    {
        _highlighter = new Highlighter();
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
        _gridBuilder = new GameObject("Builder")
            .AddComponent<GridBuilder>()
            .SetGridBuilder(_config, _grid, _cellMark, _cellCalculator);

        var buildResult = _gridBuilder
            .SetWorldOffset()
            .PopulateGridMarks()
            .SetupMirrorMapping()
            .Build();

        GridMarks = buildResult.GridMarks;
        GridMarksDimension = buildResult.GridMarksDimension;
    }

    public SpaceMark OnMouseSelect(Vector3 mouseWorldPosition, out PlacementSystem This)
    {
        This = this;
        Vector3 cellCenter = _cellCalculator.CellPosition(mouseWorldPosition, _config);
        return GridMarks[cellCenter];
    }
    public void OnMouseHover(Vector3 mouseWorldPosition) => 
        _cellCalculator.SetHover(mouseWorldPosition, _config, GridMarks, GridMarksDimension);
    public void OnMouseHoverExit() => _highlighter.StopHighlight();
}
