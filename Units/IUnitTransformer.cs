using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public interface IUnitTransformer
{
    void SetValues(GameObject a, GameObject b);
    void CreateUnit(SpaceMark target);
    void SelectUnit(SpaceMark current);
    void MoveUnit(SpaceMark current, SpaceMark target, bool CanInstantiate);
    void SwapUnits(SpaceMark current, SpaceMark target, bool CanInstantiate, bool CurrentInstantiate);
}
/*
GridMarks[position].name = $"GridMark - {x},{y}";
GridMarks[position].Dimension = new Vector2Int(x, y);
GridMarksDimension[new Vector2Int(x, y)] = GridMarks[position];

void SetupMirrorMapping()
{
    foreach (SpaceMark mark in GridMarks.Values)
    {
        int mirrorY = _gridDimensions.y - mark.Dimension.y + 1;
        mark.MirroredMark = GridMarksDimension[new Vector2Int(mark.Dimension.x, mirrorY)];
    }
}
*/