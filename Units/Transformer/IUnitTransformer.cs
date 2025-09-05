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
    void SetValues(UnitPrefabsSO units);
    void CreateUnit(SpaceMark target, string name);
    void SelectUnit(SpaceMark current, bool CurrentInstantiate);
    void MoveUnit(SpaceMark current, SpaceMark target, bool CanInstantiate, bool CurrentInstantiate);
    void SwapUnits(SpaceMark current, SpaceMark target, bool CanInstantiate, bool CurrentInstantiate);
}