using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicHealthCommand : IUnitCommand
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target)
    {
        UnitStats unitStatsCurrent = unitLogic.UnitStatsLogic;
        Debug.Log($"{unitStatsCurrent.Name} was damaged, current HP {unitStatsCurrent.CurrentHP}.");
    }
}
