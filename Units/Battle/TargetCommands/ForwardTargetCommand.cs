using UnityEngine;

public class ForwardTargetCommand : ITargetCommand
{
    public UnitStats[] Execute(UnitLogic unitLogic)
    {
        return new UnitStats[0];
    }
}
