using System.Linq;
using UnityEngine;

public class EnemyAllTargetCommand : ITargetCommand
{
    public UnitStats[] Execute(UnitLogic unitLogic)
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        return units
           .Where(unit => unit.Item1.Ownership == Owner.Enemy) 
           .Select(unit => unit.Item1)                         
           .ToArray();
    }
}
