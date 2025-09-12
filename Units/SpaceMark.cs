using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceMark : MonoBehaviour
{
    public GameObject Unit { get; set;} = null;
    public SpaceMark MirroredMark { get; set; }
    [field:SerializeField]
    public Vector2Int Dimension { get; set; }
    public GridConfig Config { get; set; }
    public SpaceMark PointerMark { get; set; } = null;
    public bool IsPointer { get; set; } = false;

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
    public GameObject Take()
    {
        if (PointerMark != null && PointerMark != this)
            return PointerMark.Take();

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
    public void SetPosition() 
    {
        if (Unit != null)
        {
            Unit.transform.position = this.transform.position;
            OccupyBySize();
            SetUnitToCenterPosition();
        }
    }
    void OccupyBySize()
    {
        var unitStats = Unit.GetComponent<UnitStats>();
        if (unitStats == null) return;
        if (unitStats.Size == new Vector2Int(1,1)) return;

        IsPointer = true;

        foreach (var grid in GridMarksDimension)
        {
            for (int x = 0; x < unitStats.Size.x; x++)
            {
                for (int y = 0; y < unitStats.Size.y; y++)
                {
                    Vector2Int targetPos = Dimension + new Vector2Int(x, y);

                    GridMarksDimension[targetPos].Unit = Unit;
                    GridMarksDimension[targetPos].PointerMark = this;
                    m_occupiedMarks.Add(GridMarksDimension[targetPos]);
                }
            }
            break; 
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
}
