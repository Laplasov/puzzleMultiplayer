using UnityEngine;

public class BasicCommand : IUnitCommand
{
    public void ExecuteOnSP(UnitLogic unitLogic)
    {
        UnitStats[] unitMark = unitLogic.TargetSP.Execute(unitLogic);
        if (unitMark == null || unitMark.Length == 0)
        {
            Debug.Log("No enemy");
            return; 
        }
        Debug.Log(unitMark[0].Name);
    }

    public void ExecuteOnSPD(UnitLogic unitLogic)
    {
    }
    public void ExecuteOnHP(UnitLogic unitLogic)
    {
    }

}
