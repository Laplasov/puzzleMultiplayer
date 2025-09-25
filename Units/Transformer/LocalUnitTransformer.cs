using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LocalUnitTransformer : MonoBehaviour, IUnitTransformer
{
    UnitPrefabsSO m_units;
    public void SetValues(UnitPrefabsSO units)
    {
        m_units = units;
    }
    public GameObject CreateUnit(SpaceMark target, string name)
    {
        var unit = m_units.GetPrefabByName(name);

        target.Unit = Instantiate(unit, target.transform.position, Quaternion.identity);
        target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
        target.SetPosition();
        return target.Unit;
    }
    public void SelectUnit(SpaceMark current, PlacementSystem board)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void MoveUnit(SpaceMark current, SpaceMark target, PlacementSystem board, PlacementSystem currentBoard)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
        target.Unit = current.Take(target);
        target.SetPosition();
    }
    public void SwapUnits(SpaceMark current, SpaceMark target, PlacementSystem board, PlacementSystem currentBoard)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
        target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

        var unit1 = target.Take(current);
        var unit2 = current.Take(target);
        var previousCurrentMark = current;

        target.Unit = unit2;
        previousCurrentMark.Unit = unit1;

        target.SetPosition();
        previousCurrentMark.SetPosition();
    }
}
