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
    GameObject CreateUnit(SpaceMark target, string name);
    void SelectUnit(SpaceMark current, PlacementSystem CurrentInstantiate);
    void MoveUnit(SpaceMark current, SpaceMark target, PlacementSystem CanInstantiate, PlacementSystem CurrentInstantiate);
    void SwapUnits(SpaceMark current, SpaceMark target, PlacementSystem CanInstantiate, PlacementSystem CurrentInstantiate);
}