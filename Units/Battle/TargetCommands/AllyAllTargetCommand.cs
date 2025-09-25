using System.Linq;
using UnityEngine;

public class AllyAllTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        Owner owner = unitLogic.gameObject.GetComponent<UnitStats>().Ownership;
            return units
               .Where(unit => unit.Item1.Ownership == owner)
               .Select(unit => unit.Item2)
               .ToArray();
    }
}
