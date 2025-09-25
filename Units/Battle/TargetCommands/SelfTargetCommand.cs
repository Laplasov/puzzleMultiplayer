using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfTargetCommand : ITargetCommand
{
    public SpaceMark[] Execute(UnitLogic unitLogic, UnitCommandConfig config)
    {
        List<SpaceMark> enemies = new List<SpaceMark>();
        enemies[0] = unitLogic.GetMySpaceMark();
        return enemies.ToArray();
    }
}
