using UnityEngine;

public interface ITargetCommand
{
    public UnitStats[] Execute(UnitLogic unitLogic);
}
