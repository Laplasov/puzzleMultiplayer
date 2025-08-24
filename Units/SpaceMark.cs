using System;
using UnityEngine;

public class SpaceMark : MonoBehaviour
{
    public GameObject Unit { get; set;} = null;
    public SpaceMark MirroredMark { get; set; }
    public Vector2Int Dimension { get; set; }
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
