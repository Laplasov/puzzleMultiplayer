using UnityEngine;

public interface IUnitCommand 
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target);

}
