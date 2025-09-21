using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpaceMark : MonoBehaviour
{
    GameObject m_unit = null;
    public GameObject Unit
    {
        get { return m_unit; }
        set
        {
            if (value != null)
                GridRegistry.Instance.OnGetUnit += OnGetUnit;
            else
                GridRegistry.Instance.OnGetUnit -= OnGetUnit;
            m_unit = value;
        }
    }
    public SpaceMark CurrentdMark { get; set; }
    public SpaceMark MirroredMark { get; set; }
    public Vector2Int Dimension { get; set; }
    public GridConfig Config { get; set; }
    public SpaceMark PointerMark { get; set; } = null;
    public bool IsPointer { get; set; } = false;
    public PlacementType Type { get; set; }
    public Dictionary<Vector2Int, SpaceMark> GridMarksDimension { get; set; } = new Dictionary<Vector2Int, SpaceMark>();

    List<SpaceMark> m_occupiedMarks = new List<SpaceMark>();
    MeshRenderer m_meshRenderer;
    Color m_color;

    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_color = m_meshRenderer.material.color;
    }
    public void SetColor(Color color) => m_meshRenderer.material.color = color;
    public void ResetColor() => m_meshRenderer.material.color = m_color;
    (UnitStats, SpaceMark) OnGetUnit() => (GetUnitStats(), this);
    public UnitStats GetUnitStats() => Unit.GetComponent<UnitStats>();
    public Vector2Int GetSizeUnit() => GetUnitStats().Size;
    public GameObject Take(SpaceMark newMark)
    {
        if (PointerMark != null && PointerMark != this)
            return PointerMark.Take(newMark);

        GameObject unit = Unit;
        Unit = null;
        PointerMark = null;

        foreach (var mark in m_occupiedMarks)
        {
            mark.Unit = null;
            mark.PointerMark = null;
        }
        m_occupiedMarks.Clear();
        return unit;
    }
    private void OnDestroy() => GridRegistry.Instance.OnGetUnit -= OnGetUnit;

    public void SetPosition() 
    {
        if (Unit != null)
        {
            Unit.transform.position = this.transform.position;
            if (Type == PlacementType.UnitHolder)
                return;
            OccupyBySize();
            SetUnitToCenterPosition();
        }
    }
    void OccupyBySize()
    {
        if (Type == PlacementType.UnitHolder) return;

        var unitStats = Unit.GetComponent<UnitStats>();
        if (unitStats == null) return;

        if (unitStats.Size == new Vector2Int(1, 1)) return;

        IsPointer = true;
        m_occupiedMarks.Clear();

        for (int x = 0; x < unitStats.Size.x; x++)
        {
            for (int y = 0; y < unitStats.Size.y; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector2Int targetPos = Dimension + new Vector2Int(x, y);

                if (GridMarksDimension.ContainsKey(targetPos))
                {
                    GridMarksDimension[targetPos].Unit = Unit;
                    GridMarksDimension[targetPos].PointerMark = this;
                    m_occupiedMarks.Add(GridMarksDimension[targetPos]);
                }
            }
        }
    }

    void SetUnitToCenterPosition()
    {
        if (Unit == null || m_occupiedMarks.Count == 0) return;
        
        float centerX = transform.position.x;
        float centerZ = transform.position.z;
        
        foreach (var mark in m_occupiedMarks)
        {
            centerX += mark.transform.position.x;
            centerZ += mark.transform.position.z;
        }
        
        centerX /= (m_occupiedMarks.Count + 1);
        centerZ /= (m_occupiedMarks.Count + 1);
        
        Unit.transform.position = new Vector3(centerX, transform.position.y, centerZ);
    }

    public bool CanPlaceUnit(SpaceMark currentUnit)
    {
        // On UnitHolder, all units are treated as 1x1 regardless of actual size
        Vector2Int unitSize = (Type == PlacementType.UnitHolder) ? new Vector2Int(1, 1) : currentUnit.GetSizeUnit();
        GameObject unitToPlace = currentUnit.Unit;

        for (int x = 0; x < unitSize.x; x++)
        {
            for (int y = 0; y < unitSize.y; y++)
            {
                Vector2Int targetPos = Dimension + new Vector2Int(x, y);
                if (!GridMarksDimension.ContainsKey(targetPos))
                    return false;

                SpaceMark mark = GridMarksDimension[targetPos];

                // Check if this mark has any unit (including pointer marks)
                if (mark.Unit != null && mark.Unit != unitToPlace)
                    return false;

                // Additional check: if this is a pointer mark pointing to a different unit
                // Only do this check if NOT on UnitHolder (since UnitHolder doesn't use pointer marks)
                if (Type != PlacementType.UnitHolder && mark.PointerMark != null && mark.PointerMark.Unit != null &&
                    mark.PointerMark.Unit != unitToPlace)
                    return false;
            }
        }
        return true;
    }

}
