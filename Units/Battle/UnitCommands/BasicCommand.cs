using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicCommand : IUnitCommand
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target)
    {
        SpaceMark[] unitMark = target.Execute(unitLogic, commandConfig);
        if (unitMark == null || unitMark.Length == 0)
        {
            Debug.Log("No enemy");
            return; 
        }
        UnitStats unitStatsTarget = unitMark[0].Unit.GetComponent<UnitStats>();
        Debug.Log(unitStatsTarget.Name);
    }
}
