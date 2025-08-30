using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlacementType { Battlefield, UnitHolder, Hided }
public class PlacementSystem : MonoBehaviour, IMouseHover, IMouseSelect
{
    [SerializeField]
    GameObject _cellIndicator;

    [SerializeField]
    GameObject _cellMark;

    [SerializeField] 
    Vector2Int _gridDimensions = new Vector2Int(5, 5);


    [SerializeField]
    bool _canInstantiate;
    bool CanInstantiate() => _canInstantiate;

    public float IndicatorOffset = 0.25f;

    float _cellHigh = 0.5f;
    float _indicatorScaleFactor = 0.5f;

    Grid _grid;
    Vector3 _yScale;
    Vector3Int _gridPosition;

    [HideInInspector]
    public GameObject MarksParent;

    public Dictionary<Vector3, SpaceMark> GridMarks; 
    Vector3 _worldOffset;

    [SerializeField]
    PlacementType _currentType;

    public static Dictionary<PlacementType, Dictionary<Vector2Int, SpaceMark>> GridRegister = new();
    public Dictionary<Vector2Int, SpaceMark> GridMarksDimension = new(); 
    Vector2Int GridOffset => new Vector2Int(_gridDimensions.x / 2, _gridDimensions.y / 2);

    void Awake()
    {
        SetIndicator();
        SetFields();
        UpdateIndicatorScale();
        SetWorldOffset();
        PopulateGridMarks();
        SetupMirrorMapping();
        GridRegister[_currentType] = GridMarksDimension;
    }
    void SetIndicator()
    {
        if (_cellIndicator == null)
        {
            _cellIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _cellIndicator.GetComponent<Collider>().enabled = false;
        }
    }
    void SetFields()
    {
        _grid = GetComponent<Grid>();
        _yScale = new Vector3(0, _cellIndicator.transform.localScale.y / 2, 0);
        GridMarks = new Dictionary<Vector3, SpaceMark>();
    }
    void UpdateIndicatorScale()
    {
        if (_grid != null)
        {
            _cellIndicator.transform.localScale = new Vector3(
                _grid.cellSize.x * _indicatorScaleFactor,
                 _cellHigh, //_cellIndicator.transform.localScale.y, 
                _grid.cellSize.z * _indicatorScaleFactor
            );
        }
    }
    void SetWorldOffset()
    {
        float offsetX = (_gridDimensions.x % 2 == 1) ? 0.5f : 0f;
        float offsetZ = (_gridDimensions.y % 2 == 1) ? 0.5f : 0f;

        if (_gridDimensions.x == 1)
            offsetX = -0.2f;

        if (_gridDimensions.y == 1)
            offsetZ = -0f;

        _worldOffset = new Vector3(offsetX, 0, offsetZ);
    }
    void PopulateGridMarks()
    {
        if (_cellMark == null) return;
        Vector2Int offset = GridOffset;
        MarksParent = new GameObject("GridMarks");
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x - offset.x, 0, y - offset.y);
                Vector3 worldPosition = _grid.GetCellCenterWorld(cellPosition) - _worldOffset;
                Vector3 position = CellPosition(worldPosition);

                GridMarks[position] = 
                    Instantiate(_cellMark, worldPosition, Quaternion.identity, MarksParent.transform)
                    .AddComponent<SpaceMark>();

                GridMarks[position].name = $"{_currentType.ToString()} - {x},{y}";
                GridMarks[position].Dimension = new Vector2Int(x, y);
                Vector2Int globalCoordinate = new Vector2Int(x , y);
                GridMarksDimension[globalCoordinate] = GridMarks[position];
            }
        }
    }
    Vector3 CellPosition(Vector3 position)
    {
        _gridPosition = _grid.WorldToCell(position + _worldOffset);
        Vector2Int offset = GridOffset;

        _gridPosition.x = Mathf.Clamp(_gridPosition.x, -offset.x, _gridDimensions.x  - offset.x - 1);
        _gridPosition.z = Mathf.Clamp(_gridPosition.z, -offset.y, _gridDimensions.y  - offset.y - 1);

        return _grid.GetCellCenterWorld(_gridPosition) - _worldOffset;
    }
    void SetupMirrorMapping()
    {
        GameObject hidedMarks;
        hidedMarks = new GameObject($"hidedMarks - {_currentType.ToString()}");
        hidedMarks.SetActive(false); 

        foreach (SpaceMark mark in GridMarks.Values)
        {
            if (_canInstantiate)
            {
                Vector2Int mirrorDimension = new Vector2Int(mark.Dimension.x, mark.Dimension.y + 1);
                GameObject mirrorObj = new GameObject($"MirrorMark {mark.Dimension.x}-{mark.Dimension.y}");
                mirrorObj.SetActive(false);

                SpaceMark newMark = mirrorObj.AddComponent<SpaceMark>();
                newMark.transform.parent = hidedMarks.transform;

                GridMarksDimension[mirrorDimension] = newMark;
                GridMarksDimension[mirrorDimension].MirroredMark = mark;
            } 
            int mirrorY = (_gridDimensions.y - 1) - mark.Dimension.y;
            mark.MirroredMark = GridMarksDimension[new Vector2Int(mark.Dimension.x, mirrorY)];
        }
    }
    public void OnMouseHover(Vector3 mouseWorldPosition)
    {
        Vector3 cellCenter = CellPosition(mouseWorldPosition);
        _cellIndicator.transform.position = cellCenter + Vector3.up * IndicatorOffset;
    }

    public SpaceMark OnMouseSelect(Vector3 mouseWorldPosition, out bool canInstantiate)
    {
        canInstantiate = CanInstantiate();
        Vector3 cellCenter = CellPosition(mouseWorldPosition);
        return GridMarks[cellCenter];
    }

}
