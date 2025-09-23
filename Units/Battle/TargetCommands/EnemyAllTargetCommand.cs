using System.Linq;
using UnityEngine;

public class EnemyAllTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);

        Owner owner = unitLogic.gameObject.GetComponent<UnitStats>().Ownership;
        if (owner == Owner.Ally)
            owner = Owner.Enemy;
        else
            owner = Owner.Ally;

            return units
               .Where(unit => unit.Item1.Ownership == owner)
               .Select(unit => unit.Item2)
               .ToArray();
    }
}
