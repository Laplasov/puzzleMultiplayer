using System;
using UnityEngine;

public class SpaceMark : MonoBehaviour
{
    public GameObject Unit { get; set;} = null;
    public SpaceMark MirroredMark { get; set; }
    public Vector2Int Dimension { get; set; }
    
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
        GameObject unit = Unit;
        Unit = null;
        return unit;
    }
    public void SetPosition() 
    {
        if (Unit != null) 
            Unit.transform.position = this.transform.position;
    }
}
