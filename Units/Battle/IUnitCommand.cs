using UnityEngine;

public interface IUnitCommand 
{
    public void ExecuteOnSP(UnitLogic unitLogic);
    public void ExecuteOnSPD(UnitLogic unitLogic);
    public void ExecuteOnHP(UnitLogic unitLogic);
}
