using UnityEngine;

public interface ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config);
}
