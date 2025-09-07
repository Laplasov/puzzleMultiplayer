using UnityEngine;

[System.Serializable]
public class GridConfig
{
    [SerializeField]
    public PlacementType currentType;
    [SerializeField]
    public Vector2Int dimensions = new Vector2Int(5, 5);
    [SerializeField]
    public float indicatorOffset = 0.25f;
    [SerializeField]
    public float cellHeight = 0.5f;
    [SerializeField]
    public float indicatorScaleFactor = 0.5f;
    [SerializeField]
    public bool canInstantiate;

    [System.NonSerialized]
    public Grid grid;
    [System.NonSerialized]
    public Vector3 worldOffset;
    [System.NonSerialized]
    public Vector3Int gridPosition;

    public Vector2Int GridOffset => new Vector2Int(dimensions.x / 2, dimensions.y / 2);

    public Vector3 GridToLocalScale(Grid grid) 
    {
        return new Vector3(
        grid.cellSize.x * indicatorScaleFactor,
            cellHeight,
        grid.cellSize.z * indicatorScaleFactor);
    }
}

public enum PlacementType { Battlefield, UnitHolder, Hided }
