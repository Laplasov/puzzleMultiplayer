using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicSpellCommand : IUnitCommand
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target)
    {
        SpaceMark[] unitMarks = target.Execute(unitLogic, commandConfig);
        if (unitMarks == null || unitMarks.Length == 0)
        {
            Debug.Log("No enemy, on spell.");
            return;
        }

        SpaceMark unitMarkCurrent = unitLogic.GetMySpaceMark();
        UnitStats unitStatsTarget = unitMarks[0].Unit.GetComponent<UnitStats>();
        UnitStats unitStatsCurrent = unitLogic.UnitStatsLogic;

        var Current = unitStatsCurrent.Name;
        var Target = unitStatsTarget.Name;

        //Debug.Log($"{Target} was enchanted by {Current}");
    }
}
