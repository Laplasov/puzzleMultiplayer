using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LocalUnitTransformer : MonoBehaviour, IUnitTransformer
{
    private GameObject m_unitA;
    private GameObject m_unitB;
    public void SetValues(GameObject unitA, GameObject unitB)
    {
        m_unitA = unitA;
        m_unitB = unitB;
    }
    public void CreateUnit(SpaceMark target)
    {
        target.Unit = Instantiate(m_unitA, target.transform.position, Quaternion.identity);
        target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
    }
    public void SelectUnit(SpaceMark current)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void MoveUnit(SpaceMark current, SpaceMark target, bool CanInstantiate)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
        target.Unit = current.Take();
        target.SetPosition();
    }
    public void SwapUnits(SpaceMark current, SpaceMark target, bool CanInstantiate, bool currentInstantiate)
    {
        current.Unit.GetComponent<MeshRenderer>().material.color = Color.white;
        target.Unit.GetComponent<MeshRenderer>().material.color = Color.white;

        var unit1 = target.Take();
        var unit2 = current.Take();
        var previousCurrentMark = current;

        target.Unit = unit2;
        previousCurrentMark.Unit = unit1;

        target.SetPosition();
        previousCurrentMark.SetPosition();
    }
}
